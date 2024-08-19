using System.Collections;
using UnityEngine;

public class BeetField : MonoBehaviour, IInteractable
{
    public GameObject soil;
    public Sprite grassSoil; // 잔디
    public Sprite normalSoil; // 기본 땅
    public Sprite diggedSoil; // 파인 땅

    public GameObject seed; // 씨앗 오브젝트
    public Animator seedAnimator; // 씨앗 애니메이터

    public Sprite coveredSoil; // 씨앗을 심고 덮은 땅
    public GameObject beet; // 작물 성장과정이 담긴 오브젝트(비트) = BeetGrowth

    public Equipment equipment;
    public EquipTool curTool;

    [SerializeField] private int needItemCode = 1102;
    [SerializeField] private ItemSO seedSO; // 심을 씨앗 아이템

    private Transform parentObj;
    public FieldData fieldDataDic;
    private string keyName;

    public SEType digSoundEffect = SEType.DIG;

    private void Start()
    {
        equipment = GameManager.Instance.Player.equipment;
        parentObj = transform.parent;
        keyName = $"{seedSO.itemCode}/{gameObject.transform.position.ToString("F2")}";
        seed.SetActive(false); // 초기에는 씨앗을 비활성화
        // 본인 데이터 있는지 확인
        if (!DataManager.Instance.fieldDataDic.ContainsKey(keyName))
        {
            // 없다면 그로스 데이터 있는지 확인
            if (!DataManager.Instance.growthDataDic.ContainsKey($"{seedSO.itemCode + 500}/{gameObject.transform.position.ToString("F2")}"))
            {
                // 없다면 본인 데이터 초기화 및 추가
                Init();
            }
            else
            {
                // 있다면 그로스 오브젝트 생성하고 본인 것 파괴
                Beet();
                Destroy(gameObject);
                return;
            }
        }
        // 본인 데이터 있다면 초기화 및 기본 설정
        fieldDataDic = DataManager.Instance.fieldDataDic[keyName];
        soil.GetComponent<SpriteRenderer>().sprite = SoilStateToSprite((int)fieldDataDic.currentState);
    }

    private void Init()
    {
        FieldData fieldData = new FieldData(seedSO.itemCode, gameObject.transform.position, keyName, SoilState.Grass);
        DataManager.Instance.fieldDataDic[keyName] = fieldData;
        DataManager.Instance.curData.fieldData.Add(fieldData);
    }

    private Sprite SoilStateToSprite(int state)
    {
        Sprite sprite = null;
        switch (state)
        {
            case 0:
                sprite = grassSoil;
                break;
            case 1:
                sprite = normalSoil;
                break;
            case 2:
                sprite = diggedSoil;
                break;
            case 3:
                sprite = coveredSoil;
                break;
        }
        return sprite;
    }

    public string GetInteractPrompt() // 오브젝트가 감지됐을때 뜨는 프롬프트 문구
    {
        if (equipment == null)
        {
            Debug.Log("장비 초기화 안됨");
            return "";
        }

        curTool = equipment.curEquipTool;

        if (curTool != null)
        {
            GameManager.Instance.Player.interaction.OnEquipUI(true); // 인터랙션 하는 곳에 다 붙여줘야됨.
            return ""; // 어떤 도구든 장착되었을 때!
        }
        else
        {
            return "삽을 장착하세요"; // 도구가 장착되지 않았을 때!
        }
    }
     
    public void OnInteract() // 인터랙트 됐을 때 일어나는 동작
    {
        curTool = equipment.curEquipTool;
        Inventory inventory = GameManager.Instance.Player.inventory;

        if (curTool != null && needItemCode == equipment.PlayerNowEquipToolCode())
        {
            if (fieldDataDic.currentState == SoilState.Digged && inventory.GetItemCount(seedSO) < 1)
            {
                Debug.Log("씨앗이 부족합니다");
                return;
            }

            if (fieldDataDic.currentState == SoilState.Grass)
            {
                GameManager.Instance.Player.animController.OnDigEvent?.Invoke();
                SoundManager.Instance.PlayEffect(digSoundEffect);
                soil.GetComponent<SpriteRenderer>().sprite = normalSoil;
                fieldDataDic.currentState = SoilState.Normal;
            }
            else if (fieldDataDic.currentState == SoilState.Normal)
            {
                GameManager.Instance.Player.animController.OnDigEvent?.Invoke();
                SoundManager.Instance.PlayEffect(digSoundEffect);
                soil.GetComponent<SpriteRenderer>().sprite = diggedSoil; // 땅이 파인다
                fieldDataDic.currentState = SoilState.Digged;
                Debug.Log("이제 씨앗을 심어보자!");
            }
            else if (fieldDataDic.currentState == SoilState.Digged)
            {
                if (inventory.GetItemCount(seedSO) >= 1)
                {
                    // 씨앗 애니메이션 트리거
                    seed.SetActive(true);
                    seedAnimator.SetTrigger("PlantBeetSeed");
                    fieldDataDic.currentState = SoilState.Covered;
                    inventory.RemovedItem(seedSO); // 인벤토리에서 씨앗 -1
                    StartCoroutine(DeactivateSeedAfterAnimation());
                }
            }
            else if (fieldDataDic.currentState == SoilState.Covered)
            {
                GameManager.Instance.Player.animController.OnDigEvent?.Invoke();
                SoundManager.Instance.PlayEffect(digSoundEffect);
                soil.GetComponent<SpriteRenderer>().sprite = coveredSoil; // 땅을 덮는다
                fieldDataDic.currentState = SoilState.Covered;
                Debug.Log("물을 뿌려보자!");
            }
        }
    }

    private IEnumerator DeactivateSeedAfterAnimation()
    {
        // 애니메이션 길이만큼 대기
        yield return new WaitForSeconds(seedAnimator.GetCurrentAnimatorStateInfo(0).length);

        seed.SetActive(false);
        Beet();
        Destroy(gameObject);// 나자신 파괴
        // 오브젝트가 없어졌으므로 데이터도 딕셔너리, 리스트에서 제거
        DataManager.Instance.fieldDataDic.Remove(keyName);
        for (int i = 0; i < DataManager.Instance.curData.fieldData.Count; i++)
        {
            if (DataManager.Instance.curData.fieldData[i].keyName == keyName)
            {
                DataManager.Instance.curData.fieldData.RemoveAt(i);
            }
        }
    }

    public void Beet()
    {
        Instantiate(beet, transform.position, transform.rotation, parentObj);
    }
}
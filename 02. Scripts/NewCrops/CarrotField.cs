using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarrotField : MonoBehaviour, IInteractable
{
    public GameObject soil;
    public Sprite grassSoil; // 잔디
    public Sprite normalSoil; // 기본 땅
    public Sprite diggedSoil; // 파인 땅

    public GameObject seed; // 씨앗 오브젝트
    public Animator seedAnimator; // 씨앗 애니메이터

    public Sprite coveredSoil; // 씨앗을 심고 덮은 땅
    public GameObject carrot; // 작물 성장과정이 담긴 오브젝트(당근) = CarrotGrowth

    public Equipment equipment;
    public EquipTool curTool;

    [SerializeField] private int needItemCode = 1102;
    [SerializeField] private ItemSO seedItem; // 심을 씨앗 아이템

    private enum SoilState
    {
        Grass,
        Normal,
        Digged,
        Covered
    }

    private SoilState currentState;

    private void Start()
    {
        equipment = GameManager.Instance.Player.equipment;
        currentState = SoilState.Grass; // 잔디상태의 땅
        soil.GetComponent<SpriteRenderer>().sprite = grassSoil;
        seed.SetActive(false); // 초기에는 씨앗을 비활성화
    }

    public string GetInteractPrompt() // 오브젝트가 감지됐을때 뜨는 프롬프트 문구
    {
        if (equipment == null)
        {
            Debug.Log("장비 초기화 안됨");
            return "장비가 초기화되지 않았습니다.";
        }

        curTool = equipment.curEquipTool;

        if (curTool != null)
        {
            GameManager.Instance.Player.interaction.OnEquipUI(true); // 인터랙션 하는 곳에 다 붙여줘야됨.
            return ""; // 어떤 도구든 장착되었을 때!
        }
        else
        {
            return "도구를 장착하세요"; // 도구가 장착되지 않았을 때!
        }
    }

    public void OnInteract() // 인터랙트 됐을 때 일어나는 동작
    {
        curTool = equipment.curEquipTool;
        Inventory inventory = GameManager.Instance.Player.inventory;

        if (curTool != null && needItemCode == equipment.PlayerNowEquipToolCode())
        {
            if (currentState == SoilState.Digged && inventory.GetItemCount(seedItem) < 1)
            {
                Debug.Log("씨앗이 부족합니다");
                return;
            }

            if (currentState == SoilState.Grass)
            {
                soil.GetComponent<SpriteRenderer>().sprite = normalSoil;
                currentState = SoilState.Normal;
            }
            else if (currentState == SoilState.Normal)
            {
                soil.GetComponent<SpriteRenderer>().sprite = diggedSoil; // 땅이 파인다
                currentState = SoilState.Digged;
                Debug.Log("이제 씨앗을 심어보자!");
            }
            else if (currentState == SoilState.Digged)
            {
                if (inventory.GetItemCount(seedItem) >= 1)
                {
                    // 씨앗 애니메이션 트리거
                    seed.SetActive(true);
                    seedAnimator.SetTrigger("PlantBeetSeed");
                    currentState = SoilState.Covered;
                    inventory.RemovedItem(seedItem); // 인벤토리에서 씨앗 -1
                    StartCoroutine(DeactivateSeedAfterAnimation());
                }
            }
            else if (currentState == SoilState.Covered)
            {
                soil.GetComponent<SpriteRenderer>().sprite = coveredSoil; // 땅을 덮는다
                currentState = SoilState.Covered;
                Debug.Log("물을 뿌려보자!");
            }
        }
    }

    private IEnumerator DeactivateSeedAfterAnimation()
    {
        // 애니메이션 길이만큼 대기
        yield return new WaitForSeconds(seedAnimator.GetCurrentAnimatorStateInfo(0).length);

        seed.SetActive(false);
        Carrot();
        Destroy(gameObject);// 나자신 파괴 
    }

    public void Carrot()
    {
        Instantiate(carrot, transform.position, transform.rotation);
    }
}

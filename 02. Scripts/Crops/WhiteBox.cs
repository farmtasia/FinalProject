using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WhiteBox : MonoBehaviour, IInteractable
{
    public GameObject blackBox; // BlackBox 오브젝트
    public Equipment equipment;
    public EquipTool curTool;

    [SerializeField] private ItemSO[] seedItems; // 심을 수 있는 씨앗 아이템 배열
    [SerializeField] private Sprite[] groundStates; // 잔디, 일반 땅, 파인 땅 이미지 배열
    [SerializeField] private GameObject[] seedPrefabs; // 씨앗 애니메이션 프리팹 배열
    private SpriteRenderer spriteRenderer; // 자식 오브젝트
    private int currentStateIndex = 0; // 현재 상태 인덱스

    private bool isSeedPlanted = false; // 씨앗이 심어졌는지 확인하는 플래그

    private void Awake()
    {
        spriteRenderer = transform.GetChild(0).GetComponent<SpriteRenderer>(); // 자식오브젝트 중 0번째를 가져온다
    }

    private void Start()
    {
        if (GameManager.Instance == null || GameManager.Instance.Player == null)
        {
            Debug.LogError("GameManager 또는 Player 객체가 초기화되지 않았습니다.");
            return;
        }

        equipment = GameManager.Instance.Player.equipment;

        if (groundStates.Length > 0)
        {
            spriteRenderer.sprite = groundStates[0]; // 초기 상태 이미지 설정
        }
    }

    void UpdateStage() // 땅파기 이미지 업데이트
    {
        if (currentStateIndex >= 0 && currentStateIndex < groundStates.Length)
        {
            spriteRenderer.sprite = groundStates[currentStateIndex];
        }
    }

    public string GetInteractPrompt() // 오브젝트가 감지됐을때 뜨는 프롬프트 문구
    {
        return "화이트박스 감지";
    }

    public void OnInteract() // 인터랙트 됐을 때 일어나는 동작
    {
        //if (GameManager.Instance == null || GameManager.Instance.Player == null)
        //{
        //    Debug.LogError("GameManager 또는 Player 객체가 초기화되지 않았습니다.");
        //    return;
        //}

        curTool = equipment.curEquipTool;
        Inventory inventory = GameManager.Instance.Player.inventory;

        if (curTool != null && curTool is Shovel)
        {
            Debug.Log("땅파기. 삽질");

            if (currentStateIndex < groundStates.Length - 1)
            {
                currentStateIndex++;
                UpdateStage();
            }

            else if (currentStateIndex == groundStates.Length - 1 && !isSeedPlanted)
            {
                if (HasAnySeedInInventory())
                {
                    StartCoroutine(PlantSeedAndActivateBlackBox());
                }
                else
                {
                    Debug.Log("씨앗이 부족합니다.");
                }
            }
        }
        else
        {
            gameObject.SetActive(true);
            blackBox.SetActive(false);
        }
    }

    private IEnumerator PlantSeedAndActivateBlackBox()
    {
        GameObject[] availableSeedPrefabs = GetAvailableSeedPrefabs();
        if (availableSeedPrefabs.Length == 0)
        {
            Debug.Log("사용 가능한 씨앗이 없습니다.");
            yield break;
        }

        PlantSeed(availableSeedPrefabs);
        yield return new WaitForSeconds(1); // 애니메이션 재생 시간만큼 대기
        isSeedPlanted = true; // 씨앗이 심어졌음을 표시
        gameObject.SetActive(false); // 나자신 비활성화
        blackBox.SetActive(true); // 블랙박스 활성화
    }

    private void PlantSeed(GameObject[] availableSeedPrefabs)
    {
        int randomIndex = Random.Range(0, availableSeedPrefabs.Length);
        GameObject seed = Instantiate(availableSeedPrefabs[randomIndex], transform); // seed 오브젝트를 WhiteBox 오브젝트 자식으로 생성
        seed.transform.localPosition = Vector3.zero; // seed의 위치를 0,0,0으로 만들어서 부모의 위치를 기준으로 설정되도록.

        ItemSO seedItem = seed.GetComponent<ItemObject>().itemData;
        UseSeedFromInventory(seedItem);

        Destroy(seed, 1); // 1초 후 애니메이션 오브젝트 삭제
    }

    private void ResetGround()
    {
        // 땅파기 상태 초기화
        currentStateIndex = 0;
        isSeedPlanted = false; // 씨앗 심기 상태 초기화
        UpdateStage();
    }

    private bool HasAnySeedInInventory()
    {
        Inventory inventory = GameManager.Instance.Player.inventory;

        if (inventory == null)
        {
            Debug.LogError("인벤토리에 접근할 수 없습니다.");
            return false;
        }

        foreach (ItemSO seedItem in seedItems)
        {
            if (inventory.GetItemCount(seedItem) > 0)
            {
                return true;
            }
        }

        return false;
    }

    private GameObject[] GetAvailableSeedPrefabs()
    {
        List<GameObject> availableSeedPrefabs = new List<GameObject>();
        Inventory inventory = GameManager.Instance.Player.inventory;

        if (inventory == null)
        {
            Debug.LogError("인벤토리에 접근할 수 없습니다.");
            return new GameObject[0];
        }

        for (int i = 0; i < seedItems.Length; i++)
        {
            if (inventory.GetItemCount(seedItems[i]) > 0)
            {
                availableSeedPrefabs.Add(seedPrefabs[i]);
            }
        }

        return availableSeedPrefabs.ToArray();
    }

    private void UseSeedFromInventory(ItemSO seedItem)
    {
        Inventory inventory = GameManager.Instance.Player.inventory;
        if (inventory != null)
        {
            inventory.RemovedItem(seedItem);
        }
    }
}

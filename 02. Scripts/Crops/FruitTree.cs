using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FruitTree : InteractObjectBase, IInteractable
{
    // isDrink: 나무에 물을 줬는지 확인하기 위함
    // isFruit: 나무에 열매가 맺혔는지 확인하기 위함
    // drinkDay: 물 받은 날짜
    // drinkTime: 물 받은 시간
    public Transform[] fruitsPos; // 열매가 맺히는 위치
    public Transform[] dropPos; // 열매가 떨어지는 위치
    [SerializeField] private SpriteRenderer[] fruitSprite;    // 열매 스프라이트 렌더러

    [SerializeField] private ItemSO fruitSO;
    private string objName;
    public TreeData treeDataDic;

    private float harvestTime;
    private float remainTime;
    private int hour;
    private int minute;

    protected override void Start()
    {
        objName = gameObject.name;
        if (!DataManager.Instance.treeDataDic.ContainsKey(objName))
            Init();
        treeDataDic = DataManager.Instance.treeDataDic[objName];

        // 과일이 맺혀있었는데 현재 이미지가 없다면 이미지 적용
        if (treeDataDic.isFruit == true)
        {
            for (int i = 0; i < fruitsPos.Length; i++)
            {
                if (fruitSprite[i].sprite == null)
                {
                    fruitSprite[i].sprite = treeDataDic.fruitSO.itemIcon;
                }
            }
        }
    }

    private void Init()
    {
        TreeData treeData = new TreeData(objName, false, false, 0, 0, null);
        DataManager.Instance.treeDataDic[objName] = treeData;
        DataManager.Instance.curData.treeData.Add(treeData);
    }

    private void Update()
    {
        if (treeDataDic.isDrink)
        {
            harvestTime = treeDataDic.drinkTime + 43200;    // 인게임 12시간 뒤 자람
            remainTime = harvestTime - (TimeManager.Instance.time + (TimeManager.Instance.days - treeDataDic.drinkDay) * 86400f);
        }        

        if (!treeDataDic.isFruit)
        {
            CreateFruits();
        }
    }

    public string GetInteractPrompt()
    {
        if (!treeDataDic.isDrink && !treeDataDic.isFruit)
        {
            return "도구를 장착하여 물을 줄 수 있습니다";
        }
        else if (treeDataDic.isDrink && !treeDataDic.isFruit)
        {
            //float harvestTime = treeDataDic.drinkTime + 43200;    // 인게임 12시간 뒤 자람
            //float remainTime = harvestTime - (TimeManager.Instance.time + (TimeManager.Instance.days - treeDataDic.drinkDay) * 86400f);

            hour = (int)(remainTime / 3600f);
            minute = (int)(Mathf.Floor(remainTime % 3600f / 60f));
            return $"성장 완료까지 {hour}시간 {minute}분 남음";
        }
        else
        {
            return "나무를 흔들어 열매를 얻을 수 있습니다\n (장착된 도구는 해제해주세요)";
        }
    }

    public void OnInteract()
    {
        if (treeDataDic.isFruit)
        {
            GameManager.Instance.Player.itemQuantity = fruitsPos.Length;

            for (int i = 0; i < fruitsPos.Length; i++)
            {
                // Instantiate(treeDataDic.fruitSO.itemPrefab, dropPos[i].position, Quaternion.identity);
                fruitSprite[i].sprite = null;
                GameManager.Instance.Player.itemdata = treeDataDic.fruitSO;
                GameManager.Instance.Player.addItem?.Invoke();
            }
            GameManager.Instance.Player.expLevel.GetExp(treeDataDic.fruitSO.expValue);
            AllReset();
        }
    }

    private void CreateFruits()
    {
        if (CreateFruitsCondition())
        {
            treeDataDic.isFruit = true;
            treeDataDic.fruitSO = GiveItem(DataManager.Instance.ItemList(EItemDetailType.FRUITS));

            for (int i = 0; i < fruitsPos.Length; i++)
            {
                fruitsPos[i].GetComponent<SpriteRenderer>().sprite = treeDataDic.fruitSO.itemIcon;
            }
        }
    }

    private bool CreateFruitsCondition()
    {
        if (treeDataDic.isDrink)
        {
            // 인게임 12시간 뒤 자람
            if (TimeManager.Instance.time + (86400 * (TimeManager.Instance.days - treeDataDic.drinkDay)) - treeDataDic.drinkTime >= 43200)
                return true;
        }
        return false;
    }

    public override ItemSO GiveItem(List<ItemSO> itemList)
    {
        return base.GiveItem(itemList);
    }

    private void AllReset()
    {
        treeDataDic.isDrink = false;
        treeDataDic.isFruit = false;
        treeDataDic.drinkDay = 0;
        treeDataDic.drinkTime = 0;
        treeDataDic.fruitSO = null;
    }
}

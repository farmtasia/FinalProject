using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class UserData    // 유저의 캐릭터 리스트
{
    public List<CharacterData> user;

    public UserData(List<CharacterData> user)
    {
        this.user = user;
    }
}

[Serializable]
public class CharacterData    // 캐릭터 정보
{
    public NameData nameData;
    public PlayerData playerData;
    public TimeData timeData;
    public List<SlotData> inventoryData;
    public string sceneData;
    public List<NPCData> npcData;
    public List<TreeData> treeData;
    public List<FieldData> fieldData;
    public List<GrowthData> growthData;
    public CollectionData collectionData;
    // TODO: 캐릭터 사진, (도입 후: 퀘스트 진행도)

    public CharacterData(NameData nameData, PlayerData playerData, TimeData timeData, List<SlotData> inventoryData, string sceneData, List<NPCData> npcData, List<TreeData> treeData, List<FieldData> fieldData, List<GrowthData> growthData, CollectionData collectionData)
    {
        this.nameData = nameData;
        this.playerData = playerData;
        this.timeData = timeData;
        this.inventoryData = inventoryData;
        this.sceneData = sceneData;
        this.npcData = npcData;
        this.treeData = treeData;
        this.fieldData = fieldData;
        this.growthData = growthData;
        this.collectionData = collectionData;
    }
}

// 클래스 별로 분류
[Serializable]
public class NameData
{
    public string userName;
    public string farmName;

    public NameData(string userName, string farmName)
    {
        this.userName = userName;
        this.farmName = farmName;
    }
}

[Serializable]
public class PlayerData    // 플레이어 정보
{
    public int gold;
    public float currentExp;
    public Vector2 pos;
    public int level;
    public bool hasCompletedTutorial;

    public PlayerData(int gold, float currentExp, Vector2 pos, int level, bool hasCompletedTutorial)
    {
        this.gold = gold;
        this.currentExp = currentExp;
        this.pos = pos;
        this.level = level;
        this.hasCompletedTutorial = hasCompletedTutorial;
    }
}

[Serializable]
public class TimeData
{
    public float time;
    public int days;

    public TimeData(float time, int days)
    {
        this.time = time;
        this.days = days;
    }
}

[Serializable]
public class SlotData    // 슬롯 하나의 아이템 정보
{
    public ItemSO item;
    public int itemCount;
    public int index;
    public bool isEquip;
    public bool isQuick;
    public int quickIndex;

    public SlotData(ItemSO item, int itemCount, int index, bool isEquip, bool isQuick, int quickIndex)
    {
        this.item = item;
        this.itemCount = itemCount;
        this.index = index;
        this.isEquip = isEquip;
        this.isQuick = isQuick;
        this.quickIndex = quickIndex;
    }
}

[Serializable]
public class NPCDefault    // ResourcesManager에서 불러오는 데이터
{
    public int npcID;
    public string name;
    public string job;
    public string imageResourcePath;
    public float initialLikeability;
}

[Serializable]
public class NPCDefaultList
{
    public List<NPCDefault> npcList;
}

[Serializable]
public class NPCData    // 저장하는 데이터
{
    public int npcID;
    public float likeability;
    public int currentEventID;
    public bool hasMetPlayer;

    public NPCData(int npcID, float likeability, int currentEventID, bool hasMetPlayer)
    {
        this.npcID = npcID;
        this.likeability = likeability;
        this.currentEventID = currentEventID;
        this.hasMetPlayer = hasMetPlayer;
    }
}

[Serializable]
public class TreeData
{
    public string treeName;
    public bool isDrink;
    public bool isFruit;
    public int drinkDay;
    public float drinkTime;
    public ItemSO fruitSO;

    public TreeData(string treeName, bool isDrink, bool isFruit, int drinkDay, float drinkTime, ItemSO fruitSO)
    {
        this.treeName = treeName;
        this.isDrink = isDrink;
        this.isFruit = isFruit;
        this.drinkDay = drinkDay;
        this.drinkTime = drinkTime;
        this.fruitSO = fruitSO;
    }
}

[Serializable]
public class FieldData
{
    public int itemCode;
    public Vector2 fieldPos;
    public string keyName;
    public SoilState currentState;

    public FieldData(int itemCode, Vector2 fieldPos, string keyName, SoilState currentState)
    {
        this.itemCode = itemCode;
        this.fieldPos = fieldPos;
        this.keyName = keyName;
        this.currentState = currentState;
    }
}

[Serializable]
public class GrowthData
{
    public int itemCode;
    public Vector2 growthPos;
    public string keyName;
    public int currentStage;
    public float wateringTime;
    public int wateringDay;
    public bool canGrowth;

    public GrowthData(int itemCode, Vector2 growthPos, string keyName, int currentStage, float wateringTime, int wateringDay, bool canGrowth)
    {
        this.itemCode= itemCode;
        this.growthPos = growthPos;
        this.keyName = keyName;
        this.currentStage = currentStage;
        this.wateringTime = wateringTime;
        this.wateringDay = wateringDay;
        this.canGrowth = canGrowth;
    }
}

[Serializable]
public class CollectionData
{
    public List<ItemSO> collectedItems;

    public CollectionData(List<ItemSO> collectedItems)
    {
        this.collectedItems = collectedItems;
    }
}
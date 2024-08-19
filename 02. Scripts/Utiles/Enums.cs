public enum EItemType
{
    CONSUMABLE, // 섭취가능한 아이템(다수 저장가능)
    EQUIPABLE // 착용가능한 아이템(하나만 저장가능)
}

public enum EItemDetailType
{
    TOOLS, // 도구, 장비류
    SEED, // 씨앗류
    TREES, // 나무
    FLOWERS, // 꽃
    VEGETABLES, // 야채류
    RESOURCE, // 자원류
    FOODS, // 요리류
    CROPS, // 작물ㅇ
    FISHES, // 생선 ㅇ
    FRUITS, // 과일 ㅇ

    COUNT
}

public enum EHarvestType
{
    FILED,
    TREE,
    COUNT
}

public enum Sound
{
    BGM,
    EFFECT,
    MAX
}

public enum BGMType
{
    NONE,
    START,
    FARM,
    VILLAGE,
    SEA
}

public enum SEType
{
    FOOTSTEPS,
    GOLD,
    CLICK,
    LEVELUP,
    DIG,
    WATERING
}

public enum UIs // 컨벤션 예외 : 파일이름으로 불러와야하기 때문
{
    Scene,
    Popup
}

public enum SceneName // 컨벤션 예외 : 씬이름으로 불러와야하기 때문
{
    StartScene,
    FarmScene,
    VillageScene,
    SeaScene
}

public enum ELayerName // 컨벤션 예외 : 레이어이름으로 불러와야하기 때문
{
    Default = 0, // 각 레이어마스크 값 할당
    Interactable = 6,
    Player = 7,
    FishingZone = 8, // 태그명도 존재하여 사용됨
    FruitTree = 12
}

public enum SoilState
{
    Grass,
    Normal,
    Digged,
    Covered
}
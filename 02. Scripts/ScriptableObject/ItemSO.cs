using UnityEngine;

[CreateAssetMenu(fileName = "Item", menuName = "New Item")]

public class ItemSO : ItemSOBase
{
    [Header("HarvestType")]
    public EHarvestType harvestType;
    public int harvestAmount;
    public float harvestSecond;
    public GameObject soil;
}
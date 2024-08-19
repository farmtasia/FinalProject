using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "SeedItem", menuName = "ScriptableObjects/SeedItem", order = 1)]
public class SeedItemSO : ScriptableObject
{
    public string beetSeed;
    public GameObject beetGrowthPrefab; // 씨앗으로 자랄 작물의 프리팹
}


public class Seed : MonoBehaviour
{
    public SeedItemSO seedData;
}

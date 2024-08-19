using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// TODO: 오브젝트 풀로 관리할 것들
// 수확물(농작물, 과일)
// 물고기
// 슬롯(상점 아이템, 로드 데이터)
// 사운드(배경음, 효과음)
// ...
// 이외 자주 생성 및 파괴하는 것들

public class ObjectPoolManager : Singleton<ObjectPoolManager>
{
    [System.Serializable]
    public class Pool
    {
        public GameObject prefab;
        public string tag;
        public int poolSize;
    }

    public List<Pool> pools = new List<Pool>();
    public Dictionary<string, Queue<GameObject>> poolDictionary;

    protected override void Awake()
    {
        base.Awake();
        poolDictionary = new Dictionary<string, Queue<GameObject>>();
        Instance.InitializePool();
    }

    public void InitializePool()
    {
        foreach (var pool in pools)
        {
            Queue<GameObject> objPool = new Queue<GameObject>();

            for (int i = 0; i < pool.poolSize; i++)
            {
                GameObject obj = Instantiate(pool.prefab, transform.parent);
                obj.SetActive(false);
                objPool.Enqueue(obj);
            }
            poolDictionary.Add(pool.tag, objPool);
        }
    }

    public GameObject SpawnFromPool(string tag)
    {
        if (!poolDictionary.ContainsKey(tag))   // 태그가 일치하는 오브젝트를 탐색
        {
            Debug.LogWarning($"{tag} 태그를 가진 풀이 없습니다.");
            return null;
        }

        GameObject obj = poolDictionary[tag].Dequeue();    // 사용하려고 큐에서 뺌
        obj.SetActive(true);
        poolDictionary[tag].Enqueue(obj);    // 재사용을 위해 큐에 다시 넣음
        return obj;
    }

    public void ReturnToPool(GameObject obj, string tag)
    {
        if (!poolDictionary.ContainsKey(tag))   // 태그가 일치하는 오브젝트를 탐색
        {
            Debug.LogWarning($"{tag} 태그를 가진 풀이 없습니다.");
            return;
        }

        obj.SetActive(false);
    }

    public Pool GetPool(string tag)
    {
        foreach (var pool in pools)
        {
            if (pool.tag == tag)
                return pool;
        }
        Debug.LogWarning($"{tag} 태그를 가진 풀이 없습니다.");
        return null;
    }
}

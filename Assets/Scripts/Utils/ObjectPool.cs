using System.Collections.Generic;
using UnityEngine;

public class ObjectPool : MonoBehaviour
{
    // 오브젝트 풀 데이터를 정의할 데이터 모음 정의
    [System.Serializable]
    public class Pool
    {
        public string tag;
        public GameObject prefab;
        public int size;
    }

    public List<Pool> Pools;
    public Dictionary<string, Queue<GameObject>> PoolDictionary;

    private void Awake()
    {
        // 인스펙터창의 Pools를 바탕으로 오브젝트풀을 만들 것. 
        // 오브젝트풀은 오브젝트마다 따로이며, pool개수를 넘어가면 강제로 끄고 새로운 오브젝트에게 할당.
        PoolDictionary = new Dictionary<string, Queue<GameObject>>();
        foreach (var pool in Pools)
        {
            // 큐는 FIFO(First-in First-out) 구조로서, 줄을 서는 것처럼 가장 오래 줄 선(enqueue) 객체가 가장 먼저 빠져 나올(dequeue) 수 있는 구조
            Queue<GameObject> objectPool = new Queue<GameObject>();
            for (int i = 0; i < pool.size; i++)
            {
                // Awake하는 순간 오브젝트풀에 들어갈 Instantitate 일어나기 때문에 터무니없는 사이즈 조심
                GameObject obj = Instantiate(pool.prefab);
                obj.SetActive(false);
                // 줄의 가장 마지막에 세움.
                objectPool.Enqueue(obj);
            }
            // 접근이 편한 Dictionary에 등록
            PoolDictionary.Add(pool.tag, objectPool);
        }
    }

    public GameObject SpawnFromPool(string tag)
    {
        // 애초에 Pool이 존재하지 않는 경우
        if (!PoolDictionary.ContainsKey(tag))
            return null;

        // 제일 오래된 객체를 재활용
        GameObject obj = PoolDictionary[tag].Dequeue();
        PoolDictionary[tag].Enqueue(obj);
        obj.SetActive(true);
        return obj;
    }
}
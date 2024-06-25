using System.Collections.Generic;
using UnityEngine;

public class MonsterSpawner : Singleton<MonsterSpawner>
{
    public List<GameObject> normalMonsters; // 일반 몬스터 프리팹 리스트
    public List<GameObject> rangedMonsters; // 원거리 몬스터 프리팹 리스트
    public List<GameObject> bossMonsters;   // 보스 몬스터 프리팹 리스트

    public GameObject GetRandomMonster()
    {
        List<GameObject> allMonsters = new List<GameObject>();
        allMonsters.AddRange(normalMonsters);
        allMonsters.AddRange(rangedMonsters);

        if (allMonsters.Count == 0)
        {
            return null;
        }

        // 랜덤으로 몬스터 프리팹을 반환
        int index = Random.Range(0, allMonsters.Count);
        return allMonsters[index];
    }

    public GameObject GetBossMonster()
    {
        if (bossMonsters == null || bossMonsters.Count == 0)
        {
            return null;
        }

        // 랜덤으로 보스 몬스터 프리팹을 반환
        int index = 0;
        return bossMonsters[index];
    }
}

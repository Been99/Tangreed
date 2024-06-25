using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class BaseStage : MonoBehaviour
{
    public List<Door> doors;

    public List<Transform> spawnPositions;
    private List<GameObject> spawnedMonsters = new List<GameObject>();

    public bool isBossStage = false;
    private bool roomCleared = false;
    private bool isPlayerInRoom = false;

    void Start()
    {
        // 동적으로 문 오브젝트를 찾기
        doors = new List<Door>(GetComponentsInChildren<Door>());

        // 스폰 위치 동적 참조
        spawnPositions = new List<Transform>();
        foreach (Transform child in transform)
        {
            if (child.name.Contains("SpawnPosition"))
            {
                spawnPositions.Add(child);
            }
        }

        if (spawnPositions.Count == 0)
        {
            roomCleared = true; // 스폰 포인트가 없으면 방이 처음부터 클리어된 상태로 설정
        }
    }

    void Update()
    {
        if (isPlayerInRoom && !roomCleared && spawnedMonsters.Count == 0)
        {
            StartCoroutine(SpawnMonsters());
        }
    }

    private IEnumerator SpawnMonsters()
    {
        if (spawnPositions.Count > 0)
        {
            CloseDoors();
        }

        // 0.5초 대기 후 몬스터 스폰
        yield return new WaitForSeconds(0.5f);

        // 몬스터 스폰
        foreach (Transform spawnPosition in spawnPositions)
        {
            if (spawnPosition.childCount == 0) // 자식이 없는 경우에만 스폰
            {
                GameObject monsterPrefab;
                if (isBossStage)
                {
                    // 보스 스테이지인 경우 보스 몬스터 소환
                    monsterPrefab = MonsterSpawner.Instance.GetBossMonster();
                }
                else
                {
                    // 일반 스테이지인 경우 일반 몬스터 소환
                    monsterPrefab = MonsterSpawner.Instance.GetRandomMonster();
                }

                if (monsterPrefab != null)
                {
                    GameObject monster = Instantiate(monsterPrefab, spawnPosition.position, Quaternion.identity);
                    monster.transform.parent = spawnPosition; // 몬스터를 SpawnPosition의 자식으로 설정
                    spawnedMonsters.Add(monster);
                    MonsterController controller = monster.GetComponent<MonsterController>();
                    if (controller != null)
                    {
                        controller.OnDeathEvent += OnMonsterDefeated;
                    }
                }
            }
        }
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            isPlayerInRoom = true;
            if (!roomCleared)
            {
                CloseDoors();
            }
            if (roomCleared)
            {
                OpenDoors();
            }
        }
    }

    public void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            isPlayerInRoom = false;
        }
    }

    public void OnPlayerEnterRoom()
    {
        isPlayerInRoom = true;

        if (roomCleared)
        {
            OpenDoors();
        }
    }

    public void OnPlayerExitRoom()
    {
        isPlayerInRoom = false;
    }

    private void CloseDoors()
    {
        foreach (var door in doors)
        {
            door.Close();
        }
    }

    private void OpenDoors()
    {
        foreach (var door in doors)
        {
            door.Open();
        }
    }

    public void OnMonsterDefeated(MonsterController monsterController)
    {
        GameObject monster = monsterController.gameObject;
        spawnedMonsters.Remove(monster);

        if (monster.transform.parent != null)
        {
            Destroy(monster.transform.parent.gameObject); // SpawnPoint도 함께 파괴
        }
        else
        {
            Destroy(monster);
        }

        if (spawnedMonsters.Count == 0)
        {
            roomCleared = true;
            OpenDoors();
        }
    }
}

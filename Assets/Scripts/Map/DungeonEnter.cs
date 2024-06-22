using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 마을에서 던전으로 이동하게 해줍니다.

public class DungeonEnter : MonoBehaviour
{
    public LayerMask playerLayer;
    public GameObject lobbyMap;
    public GameObject dungeonDoor;
    public GameObject playerGO = null;

    private void Start()
    {
        dungeonDoor.SetActive(false);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            // 플레이어의 X좌표와 Door의 Y좌표 이용 (collision.transform.position으로 했을 경우 문이 파묻혀서 생성된다.)
            Vector3 tempPos = new Vector3(collision.transform.position.x, dungeonDoor.transform.position.y);
            dungeonDoor.transform.position = tempPos;
            dungeonDoor.SetActive(true);
            playerGO = collision.gameObject;
            Invoke("EnterDungeon", 1f);
        }
    }

    public void EnterDungeon()
    {
        if (playerGO != null)
        {
            // 페이드아웃 효과
            Destroy(lobbyMap);
            // 던전 생성로직
        }
    }
}

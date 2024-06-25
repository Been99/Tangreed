using System.Collections;
using UnityEngine;

// 마을에서 던전으로 이동하게 해줍니다.

public class DungeonEnter : MonoBehaviour
{
    public LayerMask playerLayer;
    public GameObject LobbyMap;
    public GameObject lobbyDoor;
    public GameObject playerGO = null;

    public FadeEffect fadeEffect;

    private void Start()
    {
        lobbyDoor.SetActive(false);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            playerGO = collision.gameObject;
            // 플레이어의 X좌표와 Door의 Y좌표 이용 (collision.transform.position으로 했을 경우 문이 파묻혀서 생성된다.)
            Vector3 tempPos = new Vector3(playerGO.transform.position.x, lobbyDoor.transform.position.y);
            lobbyDoor.transform.position = tempPos;
            lobbyDoor.SetActive(true);

            StartCoroutine(SwitchToDungeon());
        }
    }

    private IEnumerator SwitchToDungeon()
    {
        // 페이드 인 및 페이드 아웃 시작 (1.5초 대기)
        fadeEffect.FadeInAndOut(1.5f);

        yield return new WaitForSeconds(1f);

        // LobbyMap 비활성화
        if (LobbyMap != null)
        {
            LobbyMap.SetActive(false);
        }

        // Stage1 생성
        GameObject stage1Instance = Instantiate(MapManager.Instance.mapList[0]);

        // 로비맵 비활성화
        if (LobbyMap != null)
        {
            Destroy(LobbyMap);
        }
    }
}

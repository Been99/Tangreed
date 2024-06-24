using System.Collections;
using UnityEngine;

// 마을에서 던전으로 이동하게 해줍니다.

public class DungeonEnter : MonoBehaviour
{
    public LayerMask playerLayer;
    public GameObject LobbyMap;
    public GameObject lobbyDoor;
    public GameObject playerGO = null;

    Coroutine EnterTheDungeon;

    public FadeEffect fadeEffect;

    public GameObject Stage1;

    private void Start()
    {
        lobbyDoor.SetActive(false);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            // 플레이어의 X좌표와 Door의 Y좌표 이용 (collision.transform.position으로 했을 경우 문이 파묻혀서 생성된다.)
            Vector3 tempPos = new Vector3(collision.transform.position.x, lobbyDoor.transform.position.y);
            lobbyDoor.transform.position = tempPos;
            lobbyDoor.SetActive(true);
            playerGO = collision.gameObject;

            fadeEffect.FadeIn();
            EnterTheDungeon = StartCoroutine(WaitAndEnterDungeon(1f));
            fadeEffect.DelayedFadeOut(1.5f);
        }
    }

    private IEnumerator WaitAndEnterDungeon(float waitTime)
    {
        // 플레이어 움직임 멈추는 로직
        yield return new WaitForSeconds(waitTime);
        Stage1.SetActive(true);
        Destroy(LobbyMap);
        // 플레이어 다시 움직이는 로직
    }
}

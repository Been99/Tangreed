using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    public DoorType doorType;

    public LayerMask playerLayer;
    public FadeEffect fadeEffect;
    private GameObject playerGO = null;

    public void Start()
    {
        fadeEffect = FindObjectOfType<FadeEffect>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            playerGO = collision.gameObject;
            StartCoroutine(HandleDoorTransition());
        }
    }

    private IEnumerator HandleDoorTransition()
    {
        // 페이드 인 시작
        fadeEffect.FadeIn();
        yield return new WaitForSeconds(fadeEffect.fadeDuration);

        // 플레이어 위치 변경
        switch (doorType)
        {
            case DoorType.Up:
                playerGO.transform.position += new Vector3(0, 20, 0);
                break;
            case DoorType.Down:
                playerGO.transform.position += new Vector3(0, -20, 0);
                break;
            case DoorType.Right:
                playerGO.transform.position += new Vector3(25, 0, 0);
                break;
            case DoorType.Left:
                playerGO.transform.position += new Vector3(-25, 0, 0);
                break;
        }

        // 페이드 아웃 시작
        fadeEffect.FadeOut();
        yield return new WaitForSeconds(fadeEffect.fadeDuration);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    public DoorType doorType;

    public LayerMask playerLayer;
    public FadeEffect fadeEffect;
    private GameObject playerGO = null;
    private Vector3 currentPosition;

    public GameObject doorCollider;

    public void Start()
    {
        fadeEffect = FindObjectOfType<FadeEffect>();
        doorCollider = transform.Find("MainSprite").gameObject;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            playerGO = collision.gameObject;
            currentPosition = doorCollider.transform.position;

            BaseStage baseStage = GetComponentInParent<BaseStage>();
            if (baseStage != null)
            {
                baseStage.OnPlayerEnterRoom();
            }
            StartCoroutine(HandleDoorTransition());
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            BaseStage baseStage = GetComponentInParent<BaseStage>();
            if (baseStage != null)
            {
                baseStage.OnPlayerExitRoom();
            }
        }
    }

    public void Close()
    {
        if (doorCollider != null)
        {
            doorCollider.SetActive(true);
        }
    }

    public void Open()
    {
        if (doorCollider != null)
        {
            doorCollider.SetActive(false);
        }
    }

    private IEnumerator HandleDoorTransition()
    {
        // 페이드 인 시작
        fadeEffect.FadeIn();
        yield return new WaitForSeconds(fadeEffect.fadeDuration);


        // 플레이어 위치 변경

        Vector3 offset = Vector3.zero;

        switch (doorType)
        {
            case DoorType.Up:
                offset = new Vector3(0, 22, 0);
                break;
            case DoorType.Down:
                offset = new Vector3(0, -20, 0);
                break;
            case DoorType.Right:
                offset = new Vector3(24, 0, 0);
                break;
            case DoorType.Left:
                offset = new Vector3(-24, 0, 0);
                break;
        }

        // 화면 좌표를 기준으로 플레이어 위치 변경
        Vector3 screenPos = Camera.main.WorldToScreenPoint(currentPosition + offset);
        Vector3 worldPos = Camera.main.ScreenToWorldPoint(screenPos);
        playerGO.transform.position = new Vector3(worldPos.x, worldPos.y, playerGO.transform.position.z);

        // 페이드 아웃 시작
        fadeEffect.FadeOut();
        yield return new WaitForSeconds(fadeEffect.fadeDuration);
    }
}

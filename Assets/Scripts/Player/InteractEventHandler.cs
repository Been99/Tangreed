using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractEventHandler : MonoBehaviour
{
    [Header("UI")]
    public ItemPrompt itemPrompt;
    public GameObject prompt;

    private void Start()
    {
        prompt.SetActive(false);
    }


    [SerializeField] private LayerMask targetLayer;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("충돌 중");

        if ((targetLayer.value & (1 << collision.gameObject.layer)) != 0)
        {
            // TryGetComponent로 먼저 객체 설정을 해야 함 현재 비어있음
            if (collision.TryGetComponent(out IInteractable curinteractable))
            {
                ItemSO curItemSO = curinteractable.GetItemData();

                itemPrompt.SetItemPrompt(curItemSO.itemName, curItemSO.itemDescription);
            }
        }
    }
    // exit 되거나 인벤토리로 들어가면 curinteractable를 비워야함

    // TODO : 일정 시간이 지나면 인벤토리로 이동할 수 있게
    // 타이머 기능을 사용
    // 여기서 특정시간을 변수로 설정하여 시간이 초과되면 인벤토리로 넘어갈 수 있도록
    // 타이머 관련 내용은 코루틴 참고
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.TryGetComponent(out IInteractable curinteractable))
        {
            itemPrompt.DisablePrompt();
        }
    }


}

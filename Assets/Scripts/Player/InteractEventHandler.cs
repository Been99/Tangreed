using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InteractEventHandler : MonoBehaviour
{
    [Header("UI")]
    public ItemPrompt itemPrompt;
    public GameObject prompt;
    public GameObject interaction;

    [SerializeField] private LayerMask targetLayer;
    [SerializeField] private LayerMask itemBoxLayer;

    private ItemSO curItemSO;

    [SerializeField] float waitTime;

    // 임의설정 <여기부터>
    public GameObject inventoryObj;
    private bool activeInventory = false;
    // <여기까지>

    private void Start()
    {
        prompt.SetActive(false);
        interaction.SetActive(false);
        inventoryObj.SetActive(activeInventory); // 임의설정
    }

    // 임의설정 <여기부터>
    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.I))
        {
            Debug.Log("인벤토리창");
            activeInventory = !activeInventory;
            inventoryObj.SetActive(activeInventory);
        }
    }
    // <여기까지>

    // InputController 스크립트에 나중에 갖다붙이기
    public void OnInteract(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Started)
        {
            // TODO : (조건문)아이템 박스 애니메이션 실행 및 아이템 드랍
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("충돌");
        GameObject itemObject = collision.gameObject;

        if ((targetLayer.value & (1 << itemObject.layer)) != 0)
        {
            // TryGetComponent로 먼저 객체를 가져옴
            if (itemObject.TryGetComponent(out IInteractable curinteractable))
            {
                curItemSO = curinteractable.GetItemData();
                GameManager.Instance.itemSO = curItemSO;
                GameManager.Instance.addItem?.Invoke();
                itemPrompt.SetItemPrompt(curItemSO.itemName, curItemSO);
                Destroy(itemObject);
                StartCoroutine(ItemInteractionHandle());
            }
        }
        else if ((itemBoxLayer.value & (1 << itemObject.layer)) != 0)
        {
            interaction.SetActive(true);
        }
    }

    private IEnumerator ItemInteractionHandle()
    {
        yield return new WaitForSeconds(waitTime);
        prompt.SetActive(false);
        GameManager.Instance.itemSO = null;
    }
}

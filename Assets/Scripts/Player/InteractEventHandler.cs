using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class InteractEventHandler : MonoBehaviour
{
    [Header("UI")]
    public ItemPrompt itemPrompt;
    public GameObject prompt;
    // public GameObject interaction;
    public GameObject inventoryObj;
    public ItemBox itemBox;

    [SerializeField] private LayerMask targetLayer;
    [SerializeField] private LayerMask itemBoxLayer;
    [SerializeField] float waitTime;

    private ItemSO curItemSO;

    private void Start()
    {
        prompt.SetActive(false);
        // interaction.SetActive(false);
        inventoryObj.SetActive(false);
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
                GameManager.Instance.ownedItems.Add(curItemSO);
                GameManager.Instance.addItem?.Invoke();
                itemPrompt.SetItemPrompt(curItemSO.itemName, curItemSO);
                Destroy(itemObject);
                StartCoroutine(ItemInteractionHandle());
            }
        }
        else if ((itemBoxLayer.value & (1 << itemObject.layer)) != 0)
        { 
            itemBox.StartAnim();
            StartCoroutine(OpenItemBox(itemObject));
        }
    }

    private IEnumerator ItemInteractionHandle()
    {
        yield return new WaitForSeconds(waitTime);
        prompt.SetActive(false);
        GameManager.Instance.itemSO = null;
    }

    private IEnumerator OpenItemBox(GameObject go)
    {
        yield return new WaitForSeconds(waitTime);
        itemBox.OpenItemBox();
        Destroy(go);
    }
}

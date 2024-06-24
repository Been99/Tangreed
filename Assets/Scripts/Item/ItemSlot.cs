using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ItemSlot : MonoBehaviour, IDropHandler
{
    public ItemSO itemSO;
    public Image iconImage;
    public int index;
    public UIInventory inventory;

    public void SetSlot()
    {
        iconImage.sprite = itemSO.icon;
    }

    public void ClearSlot()
    {
        itemSO = null;
    }

    public void OnDrop(PointerEventData eventData)
    {
        Debug.Log("아이템 드롭함");
        if(eventData.pointerDrag != null)
        {
            eventData.pointerDrag.GetComponent<RectTransform>().anchoredPosition = GetComponent<RectTransform>().anchoredPosition;
        }
    }
}

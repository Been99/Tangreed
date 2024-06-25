using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ItemSlot : MonoBehaviour
{
    public ItemSO itemSO;
    public Image iconImage;
    public int index;
    public UIInventory inventory;

    public void SetSlot()
    {
        Debug.Log("확인");
        iconImage.sprite = itemSO.icon;       
    }

    public void ClearSlot()
    {
        itemSO = null;
    }
}

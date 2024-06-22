using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class ItemSlot : MonoBehaviour
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
}

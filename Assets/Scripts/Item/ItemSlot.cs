using UnityEngine;
using UnityEngine.UI;

public class ItemSlot : MonoBehaviour
{
    public ItemSO itemSO;
    public Image iconImage;
    public int index;

    public UIInventory inventory;

    public void Set()
    {
        iconImage.enabled = true;
        iconImage.gameObject.SetActive(true);
        iconImage.sprite = itemSO.icon;
    }

    public void Clear()
    {
        itemSO = null;
        iconImage.enabled = false;
        iconImage.gameObject.SetActive(false);
    }
}

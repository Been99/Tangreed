using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemObject : MonoBehaviour, IInteractable
{
    public ItemSO itemdata;

    public string GetItemNamePrompt()
    {
        string str = $"{itemdata.name}";
        return str;
    }

    public string GetItemDescriptionPrompt()
    {
        string str = $"{itemdata.itemDescription}";
        return str;
    }

    public ItemSO GetItemData()
    {
        return itemdata;
    }

    public void GetItemInteract()
    {
        // TODO : 아이템 데이터 인벤토리로 바로 넘어갈 수 있도록
        Destroy(gameObject);
    }
}

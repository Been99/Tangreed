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
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemObject : MonoBehaviour, IInteractable
{
    public ItemSO itemdata;

    public string GetInteractPrompt()
    {
        // TODO : 임의로 설정 추후 바꿔야 할 듯
        string str = $"{itemdata.name}\n{itemdata.itemDescripttion}";
        return str;
    }
}

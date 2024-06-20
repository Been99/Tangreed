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

    // TODO : 아이템에 명칭과 간단한 설명만 뜨고 별도 상호작용 없이 인벤토리로 저장됨
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIInventory : MonoBehaviour
{
    public ItemSlot[] slots;
    public Transform getItemSlots;

    public ItemSO selectedItem;
    public int selectedItemIndex;

    // TODO : 게임매니저 생성 후 연결해야 함

    private void Start()
    {
        slots = new ItemSlot[getItemSlots.childCount];

        for(int i = 0; i < slots.Length; i++)
        {
            slots[i] = getItemSlots.GetChild(i).GetComponent<ItemSlot>();
            slots[i].index = i;
            slots[i].inventory = this;
        }
    }


}

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

    void AddItem(ItemSO itemdata)
    {
        ItemSlot emptySlot = GetEmptySlot();

        if(emptySlot != null)
        {
            emptySlot.itemSO = itemdata;
            UpdateUI();
            return;
        }
    }

    public void UpdateUI()
    {
        for(int i = 0; i < slots.Length;i++)
        {
            if (slots[i].itemSO != null)
            {
                slots[i].Set();
            }
            else
            {
                slots[i].Clear();
            }
        }
    }

    ItemSlot GetEmptySlot()
    {
        for(int i=0; i < slots.Length; i++)
        {
            if (slots[i].itemSO != null)
            {
                return slots[i];
            }
        }
        return null;
    }
}

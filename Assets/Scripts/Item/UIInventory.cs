using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIInventory : MonoBehaviour
{
    [Header("Equip(1)")]
    public Transform equip1ItemSlots;
    public ItemSlot[] equip1Slots;

    [Header("Equip(2)")]
    public Transform equip2ItemSlots;
    public ItemSlot[] equip2Slots;

    [Header("EquipAccessory")]
    public Transform equipAccItemSlots;
    public ItemSlot[] equipAccSlots;

    [Header("GetItem")]
    public Transform getItemSlots;
    public ItemSlot[] itemSlots;

    public GameObject inventoryWindow;


    private void Awake()
    {
        GameManager.Instance.Inventory = this;

        equip1Slots = new ItemSlot[equip1ItemSlots.childCount];
        equip2Slots = new ItemSlot[equip2ItemSlots.childCount];
        equipAccSlots = new ItemSlot[equipAccItemSlots.childCount];
        itemSlots = new ItemSlot[getItemSlots.childCount];

        SetInventorySlot(equip1Slots, equip1ItemSlots);
        SetInventorySlot(equip2Slots, equip2ItemSlots);
        SetInventorySlot(equipAccSlots, equipAccItemSlots);
        SetInventorySlot(itemSlots, getItemSlots);
    }

    private void Start()
    {
        GameManager.Instance.addItem += ItemTypeCheckAfterAdd;

        inventoryWindow.SetActive(false);       
    }

    void SetInventorySlot(ItemSlot[] slots, Transform itemslots)
    {
        for (int i = 0; i < slots.Length; i++)
        {
            slots[i] = itemslots.GetChild(i).GetComponent<ItemSlot>();
            slots[i].index = i;
            slots[i].inventory = this;
        }
    }

    void ItemTypeCheckAfterAdd()
    {
        ItemSO itemData = GameManager.Instance.itemSO;

        // 확인용
        if(itemData == null)
        {
            Debug.Log("추가할 아이템 없음");
            return;
        }

        // 아이템의 타입에 따라 인벤토리의 위치 지정해야함
        // 1) 아이템의 타입이 무기이고, 슬롯에 빈자리가 있을 때
        // 2) 아이템의 타입이 방어구이고, 슬롯에 빈자리가 있을 때
        // 3) 아이템의 타입이 악세서리이고, 슬롯에 빈자리가 있을 때 
        // 4) 위의 무기, 방어구, 악세서리 슬롯이 모두 차있고, 일반 슬롯이 비어 있을 때
        // 5) 일반 슬롯까지 모두 찼을 때

        GameManager.Instance.itemSO = null;
    }

    public void UpdateUI()
    {
        for(int i = 0; i < itemSlots.Length;i++)
        {
            if (itemSlots[i].itemSO != null)
            {
                itemSlots[i].Set();
            }
            else
            {
                itemSlots[i].Clear();
            }
        }
    }

    ItemSlot GetEmptySlot()
    {
        for(int i=0; i < itemSlots.Length; i++)
        {
            if (itemSlots[i].itemSO != null)
            {
                return itemSlots[i];
            }
        }
        return null;
    }
}

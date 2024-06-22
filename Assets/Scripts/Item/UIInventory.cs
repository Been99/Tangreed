using System.Collections.Generic;
using UnityEngine;

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

    [SerializeField] private GameObject inventoryWindow;


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
        GameManager.Instance.addItem += AddItem;
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

    void AddItem()
    {
        ItemSO itemData = GameManager.Instance.itemSO;

        switch (itemData.itemType)
        {
            case EItemType.Weapon:
                EquipItemSlotSet(equip1Slots, equip2Slots, 0, itemData);
                break;
            case EItemType.armor:
                EquipItemSlotSet(equip1Slots, equip2Slots, 1, itemData);
                break;
            case EItemType.Accessory:
                AccItemSlotSet(equipAccSlots, itemData);
                break;
            default:
                UnEquipItemSlotSet(itemData);
                break;
        }
    }

    private void EquipItemSlotSet(ItemSlot[] slots, ItemSlot[] slots2, int index, ItemSO so)
    {
        if (slots[index].itemSO == null && slots2[index].itemSO == null)
        {
            slots[index].itemSO = so;
            slots[index].SetSlot();
        }
        else if(slots[index].itemSO != null && slots2[index].itemSO == null)
        {
            slots2[index].itemSO = so;
            slots2[index].SetSlot();
        }
        else
            UnEquipItemSlotSet(so);
    }

    private void AccItemSlotSet(ItemSlot[] slots, ItemSO so)
    {
        ItemSlot emptyAccSlot = GetEmptySlot(slots);

        if (emptyAccSlot != null)
        {
            emptyAccSlot.itemSO = so;
            emptyAccSlot.SetSlot();
        }
        else
            UnEquipItemSlotSet(so);
    }

    private void UnEquipItemSlotSet(ItemSO so)
    {
        Debug.Log("꽉차서 여기로");
        ItemSlot emptySlot = GetEmptySlot(itemSlots);

        if (emptySlot != null)
        {
            emptySlot.itemSO = so;
            emptySlot.SetSlot();
        }
        else
            Debug.Log("빈 슬롯이 없음");
    }

    public ItemSlot GetEmptySlot(ItemSlot[] slots)
    {
        for (int i = 0; i < slots.Length; i++)
        {
            if (slots[i].itemSO == null)
            {
                return slots[i];
            }
        }
        return null;
    }
}

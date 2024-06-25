using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    public UIInventory Inventory;
    public ItemSO itemSO;
    public Action addItem;
    public List<ItemSO> ownedItems = new List<ItemSO>();
}

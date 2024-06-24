using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : Singleton<GameManager>
{
    public UIInventory Inventory;
    public ItemSO itemSO;
    public Action addItem;
    [SerializeField] private GameObject fadeEffect;

    private void Start()
    {
        Inventory = GetComponent<UIInventory>();
        fadeEffect.SetActive(true);
    }
}

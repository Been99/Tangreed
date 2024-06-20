using System;
using UnityEngine;


[Serializable]
public class ItemStat
{
    public EItemStat itemStat;
    public float statValue;
}


[CreateAssetMenu(fileName = "ItemSO", menuName = "SO/ItemSO")]
public class ItemSO : ScriptableObject
{
    [Header("Info")]
    public string itemName; // 아이템 이름
    public string itemDescription; // 아이템 설명
    public EItemType itemType; // 아이템 타입
    public Sprite icon; // 아이템 이미지

    [Header("ItemStat")]
    public ItemStat[] itemStats;

    [Header("ItemPrice")]
    public int itemPrice; // 아이템 가격

    // TODO : 더 필요한 내용 확인 후 적용
}

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
    public GameObject dropPrefab;

    [Header("ItemStat")]
    public ItemStat[] itemStats;

    [Header("ItemPrice")]
    public int itemPrice; // 아이템 가격

    // 셋팅 시 공격력과 방어력은 함께할 수 없고 공속과 이속은 함께할 수 없다는 걸 전제로 두
}

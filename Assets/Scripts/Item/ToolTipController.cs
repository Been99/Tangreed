using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ToolTipController : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public ToolTip toolTip;
    public Image outLine;
    public ItemSO itemData;


    void Start()
    {
        outLine.enabled = false;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        itemData = GetComponent<ItemSlot>().itemSO;

        if (itemData != null )
        {
            outLine.enabled = true;
            toolTip.gameObject.SetActive(true);
            toolTip.ShowToolTipBasicData(itemData.icon, itemData.itemName, itemData.itemDescription);
            ItemStatCheck();
        }
    }

    public void ItemStatCheck()
    {
        // TODO : 한번 불러온 자료의 데이터가 계속 지속되는 버그
        if (itemData.itemStats != null)
        {
            for (int i = 0; i < itemData.itemStats.Length; i++)
            {
                switch (itemData.itemStats[i].itemStat)
                {
                    case EItemStat.Strength:
                        toolTip.itemStrengthText.text = "공격력 : " + itemData.itemStats[i].statValue;
                        break;
                    case EItemStat.Defensive:
                        toolTip.itemDefensiveText.text = "방어력 : " + itemData.itemStats[i].statValue;
                        break;
                    case EItemStat.AttackSpeed:
                        toolTip.itemAttackSpeedText.text = "공격속도 : " + itemData.itemStats[i].statValue;
                        break;
                    case EItemStat.MovingSpeed:
                        toolTip.itemMovingSpeedText.text = "이동속도 : " + itemData.itemStats[i].statValue;
                        break;
                }
            }
        }       
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        outLine.enabled = false;
        toolTip.gameObject.SetActive(false);
        toolTip.itemStrengthText.text = "공격력 : ";
        toolTip.itemDefensiveText.text = "방어력 : ";  
        toolTip.itemAttackSpeedText.text = "공격속도 : ";
        toolTip.itemMovingSpeedText.text = "이동속도 : ";
    }
}

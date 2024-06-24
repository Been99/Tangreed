using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ToolTipController : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public ToolTip toolTip;
    public Image outLine;
    public ItemSO itemData;

    // 추후 삭제예정
    public string power;
    public string defesive;

    void Start()
    {
        outLine.enabled = false;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        itemData = GetComponent<ItemSlot>().itemSO;

        if(itemData != null )
        {
            outLine.enabled = true;
            toolTip.gameObject.SetActive(true);
            toolTip.ShowToolTip(itemData.icon, itemData.itemName, power, defesive, itemData.itemDescription);
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        outLine.enabled = false;
        toolTip.gameObject.SetActive(false);
    }
}

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
            Debug.Log("마우스 인식");
            outLine.enabled = true;
            toolTip.gameObject.SetActive(true);
            toolTip.ShowToolTip(itemData.icon, itemData.itemName, power, defesive, itemData.itemDescription);
        }
        else
            Debug.Log("빈슬롯");
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        Debug.Log("마우스 해제");
        outLine.enabled = false;
        toolTip.gameObject.SetActive(false);
    }
}

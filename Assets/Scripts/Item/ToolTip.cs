using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ToolTip : MonoBehaviour
{
    public Image itemImage;
    public TextMeshProUGUI itemNameText;
    public TextMeshProUGUI itemDescriptionText;

    public TextMeshProUGUI itemStrengthText;
    public TextMeshProUGUI itemDefensiveText;
    public TextMeshProUGUI itemAttackSpeedText;
    public TextMeshProUGUI itemMovingSpeedText;

    public RectTransform toolTipObj;
    private RectTransform toolTipRect;

    private void Start()
    {
        toolTipRect = GetComponent<RectTransform>();
        toolTipRect.gameObject.SetActive(false);            
    }

    private void Update()
    {
        // UI 캔버스 상 마우스포지션을 활용할 때 참고!
        if (toolTipObj.gameObject.activeSelf)
        {
            Vector2 localPosition;
            Vector2 mousePos = Input.mousePosition;
            RectTransformUtility.ScreenPointToLocalPointInRectangle(toolTipRect, mousePos, null, out localPosition);
            localPosition.x -= toolTipObj.sizeDelta.x * 0.65f;
            toolTipObj.anchoredPosition = localPosition;
        }
    }

    public void ShowToolTipBasicData(Sprite itemImg, string itemName, string itemDes)
    {
        itemImage.sprite = itemImg;
        itemNameText.text = itemName;
        itemDescriptionText.text = itemDes;
    }
    
    // TODO : 툴팁이 켜져있는 상태로 인벤토리를 닫으면 툴팁도 같이 꺼지게
}



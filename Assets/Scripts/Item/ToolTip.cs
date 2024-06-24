using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ToolTip : MonoBehaviour
{
    public Image itemImage;
    public TextMeshProUGUI itemNameText;
    public TextMeshProUGUI itemStrengthText;
    public TextMeshProUGUI itemDefensiveText;
    public TextMeshProUGUI itemDescriptionText;
    public RectTransform toolTipObj;
    private RectTransform toolTipRect;

    // TODO : 아이템 스탯 어디까지 툴팁에 표현할건지?

    private void Start()
    {
        toolTipRect = GetComponent<RectTransform>();
        toolTipRect.gameObject.SetActive(false);
    }

    private void Update()
    {
        if (toolTipObj.gameObject.activeSelf)
        {
            Vector2 localPosition;
            Vector2 mousePos = Input.mousePosition;
            RectTransformUtility.ScreenPointToLocalPointInRectangle(toolTipRect, mousePos, null, out localPosition);
            localPosition.x -= toolTipObj.sizeDelta.x * 0.65f;
            toolTipObj.anchoredPosition = localPosition;
            // ** 정리 필요함 -> 앞으로 잘 쓸 수 있을 듯
        }
    }

    public void ShowToolTip(Sprite itemImg, string itemName, string attackStrength, string defensive, string itemDes)
    {
        itemImage.sprite = itemImg;
        itemNameText.text = itemName;
        itemStrengthText.text = attackStrength;
        itemDefensiveText.text = defensive;
        itemDescriptionText.text = itemDes;
    }
    
    // TODO : 툴팁이 켜져있는 상태로 인벤토리를 닫으면 툴팁도 같이 꺼지게
}



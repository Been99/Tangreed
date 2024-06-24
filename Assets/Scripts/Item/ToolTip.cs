using System.Collections;
using System.Collections.Generic;
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
    public GameObject toolTipObj;

    // TODO : 아이템 스탯 어디까지 툴팁에 표현할건지?

    private void Start()
    {
        toolTipObj.SetActive(false);
    }

    private void Update()
    {
        // TODO : 마우스 위치에 따라 툴팁이 켜지게 하기
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



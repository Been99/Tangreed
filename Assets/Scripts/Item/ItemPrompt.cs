using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ItemPrompt : MonoBehaviour
{
    [SerializeField] private Image promptBG;
    [SerializeField] private TextMeshProUGUI itemNameText;
    [SerializeField] private Image itemImage;

    public void SetItemPrompt(string itemName, ItemSO itemSO)
    {
        itemNameText.text = itemName;
        itemImage.sprite = itemSO.icon;
        promptBG.gameObject.SetActive(true);
    }
}

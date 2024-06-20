using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ItemPrompt : MonoBehaviour
{
    [SerializeField] private Image promptBG;
    [SerializeField] private TextMeshProUGUI itemNameText;
    [SerializeField] private TextMeshProUGUI itemDescriptionText;

    public void SetItemPrompt(string itemName, string itemDescription)
    {
        itemNameText.text = itemName;
        itemDescriptionText.text = itemDescription;
        promptBG.gameObject.SetActive(true);
    }

    public void DisablePrompt()
    {
        promptBG.gameObject.SetActive(false);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ItemDragDrop : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public ToolTip toolTip;
    private Canvas canvas;
    private RectTransform rectTransform;
    private CanvasGroup canvasGroup;

    private void Awake()
    {
        canvas = GameManager.Instance.Inventory.GetComponent<Canvas>();
        rectTransform = GetComponent<RectTransform>();
        canvasGroup = GetComponent<CanvasGroup>();
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        Debug.Log("클릭중");
        
        canvasGroup.blocksRaycasts = false;
        toolTip.gameObject.SetActive(false);
    }

    public void OnDrag(PointerEventData eventData)
    {
        Debug.Log("드래그중");
        rectTransform.anchoredPosition += eventData.delta / canvas.scaleFactor;
        canvasGroup.alpha = 0.6f;
        toolTip.gameObject.SetActive(false);

    }

    public void OnEndDrag(PointerEventData eventData)
    {
        Debug.Log("클릭끝");
        canvasGroup.alpha = 1f;
        canvasGroup.blocksRaycasts = true;
    }

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Door : MonoBehaviour
{
    public GameObject uiInteraction;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            // Interaction UI띄우기
            if (uiInteraction != null) uiInteraction.SetActive(true);

            // 다음 방으로 이동 로직
        }
    }
    
    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            // Interaction UI없애기
            if (uiInteraction != null) uiInteraction.SetActive(false);
        }
    }
}

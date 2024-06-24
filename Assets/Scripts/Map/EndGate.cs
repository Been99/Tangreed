using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class EndGate : MonoBehaviour
{
    public TextMeshPro doorText;
    public Image interactionImage;

    public LayerMask playerLayer;

    public FadeEffect fadeEffect;

    public GameObject Stage1;
    public GameObject Stage2;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            doorText.enabled = true;
            interactionImage.enabled = true;

            // Interaction 하면 페이드인 페이드아웃 효과, Stage1 파괴하고, Stage2 활성화 코루틴 이용
            
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        doorText.enabled = false;
        interactionImage.enabled = false;
    }

}

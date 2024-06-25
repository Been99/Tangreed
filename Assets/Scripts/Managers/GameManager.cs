using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : Singleton<GameManager>
{
    public UIInventory Inventory;
    public ItemSO itemSO;
    public Action addItem;
    public List<ItemSO> ownedItems = new List<ItemSO>();
    public GameObject clearUICanvas;
    public float fadeDuration = 1.5f;

    public void HandleBatLialDeath()
    {
        StartCoroutine(ActivateUIAfterDelay(3f));
    }

    private IEnumerator ActivateUIAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        clearUICanvas.SetActive(true);
        StartCoroutine(FadeInUI());
        yield return new WaitForSeconds(3f + fadeDuration);
        StartCoroutine(FadeOutUI());
    }

    private IEnumerator FadeInUI()
    {
        CanvasGroup canvasGroup = clearUICanvas.GetComponent<CanvasGroup>();
        if (canvasGroup == null)
        {
            canvasGroup = clearUICanvas.AddComponent<CanvasGroup>();
        }

        float startAlpha = canvasGroup.alpha;
        float rate = 1.0f / fadeDuration;
        float progress = 0.0f;

        while (progress < 1.0f)
        {
            canvasGroup.alpha = Mathf.Lerp(startAlpha, 1, progress);
            progress += rate * Time.deltaTime;
            yield return null;
        }

        canvasGroup.alpha = 1;
    }

    private IEnumerator FadeOutUI()
    {
        CanvasGroup canvasGroup = clearUICanvas.GetComponent<CanvasGroup>();
        if (canvasGroup == null)
        {
            canvasGroup = clearUICanvas.AddComponent<CanvasGroup>();
        }

        float startAlpha = canvasGroup.alpha;
        float rate = 1.0f / fadeDuration;
        float progress = 0.0f;

        while (progress < 1.0f)
        {
            canvasGroup.alpha = Mathf.Lerp(startAlpha, 0, progress);
            progress += rate * Time.deltaTime;
            yield return null;
        }

        canvasGroup.alpha = 0;
        clearUICanvas.SetActive(false);
        SceneManager.LoadScene("EndingScene");
    }
}


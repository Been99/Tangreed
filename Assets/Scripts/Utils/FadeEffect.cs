using System.Collections;
using UnityEngine;

// CanvasGroup과 코루틴을 이용하여 자연스러운 전환구현 (페이크로딩)
public class FadeEffect : MonoBehaviour
{
    public CanvasGroup canvasGroup;
    public float fadeDuration = 1.0f;

    void Start()
    {
        // 초기 상태 설정
        canvasGroup.alpha = 0;
    }

    public void FadeIn()
    {
        StartCoroutine(FadeCanvasGroup(0, 1));
    }

    public void FadeOut()
    {
        StartCoroutine(FadeCanvasGroup(1, 0));
    }

    private IEnumerator FadeCanvasGroup(float start, float end)
    {
        float elapsedTime = 0.0f;

        while (elapsedTime < fadeDuration)
        {
            canvasGroup.alpha = Mathf.Lerp(start, end, elapsedTime / fadeDuration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        canvasGroup.alpha = end;
    }

    public void DelayedFadeOut(float delay)
    {
        StartCoroutine(DelayedFadeOutCoroutine(delay));
    }

    private IEnumerator DelayedFadeOutCoroutine(float delay)
    {
        yield return new WaitForSeconds(delay);
        FadeOut();
    }
}

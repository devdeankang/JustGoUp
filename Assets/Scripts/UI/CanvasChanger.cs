using System.Collections;
using UnityEngine;
using DG.Tweening;

public class CanvasChanger : MonoBehaviour
{
    private readonly WaitForSeconds wait = new WaitForSeconds(0.3f);

    public void ShowCanvas(CanvasGroup canvasGroup)
    {
        StartCoroutine(FadeCanvas(canvasGroup, 0f, 1f, true));
    }

    public void HideCanvas(CanvasGroup canvasGroup)
    {
        StartCoroutine(FadeCanvas(canvasGroup, 1f, 0f, false));
    }

    private IEnumerator FadeCanvas(CanvasGroup canvasGroup, float startAlpha, float endAlpha, bool active)
    {
        canvasGroup.alpha = startAlpha;
        if (active) canvasGroup.gameObject.SetActive(true);
        yield return wait;
        canvasGroup.DOFade(endAlpha, 0.3f);
        yield return wait;
        canvasGroup.gameObject.SetActive(active);
    }
}
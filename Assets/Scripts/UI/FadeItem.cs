using UnityEngine;
using DG.Tweening;
using System.Collections;

public class FadeItem : MonoBehaviour
{
    public float startDelay = 0.3f;
    public float fadeDuration = 0.3f;
    public CanvasGroup canvasGroup;

    void OnEnable()
    {
        SetAlpha(0f);
        StartCoroutine(FadeIn());
    }

    void OnDisable()
    {
        SetAlpha(0f);
    }

    private IEnumerator FadeIn()
    {
        yield return new WaitForSeconds(startDelay);
        canvasGroup.DOFade(1f, fadeDuration);
    }

    private void SetAlpha(float alpha)
    {
        if (canvasGroup != null)
            canvasGroup.alpha = alpha;
    }
}
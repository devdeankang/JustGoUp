using System.Collections;
using UnityEngine;
using DG.Tweening;

public class Dialog : MonoBehaviour
{
    public float animDuration = 0.2f;
    public GameObject dialogContent;

    public void ShowDialog()
    {
        if (dialogContent == null) return;
        dialogContent.SetActive(false);
        gameObject.SetActive(true);
        dialogContent.transform.localScale = Vector3.zero;
        dialogContent.SetActive(true);
        dialogContent.transform.DOScale(Vector3.one, animDuration);
    }

    public void HideDialog()
    {
        if (dialogContent == null) return;
        dialogContent.transform.DOScale(Vector3.zero, animDuration);
        StartCoroutine(HideAfterAnimation());
    }

    private IEnumerator HideAfterAnimation()
    {
        yield return new WaitForSeconds(animDuration);
        gameObject.SetActive(false);
    }
}
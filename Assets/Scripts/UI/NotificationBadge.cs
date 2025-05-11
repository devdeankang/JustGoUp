using UnityEngine;
using DG.Tweening;
using System.Collections;

public class NotificationBadge : MonoBehaviour
{
    public float showDelay = 2f;

    void OnEnable()
    {
        transform.localScale = Vector3.zero;
    }

    void Start()
    {
        StartCoroutine(ShowAfterDelay());
    }

    private IEnumerator ShowAfterDelay()
    {
        yield return new WaitForSeconds(showDelay);
        transform.DOScale(1f, 0.3f);
    }
}
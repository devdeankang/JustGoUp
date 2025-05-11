using UnityEngine;
using UnityEngine.SceneManagement;
using DG.Tweening;
using System.Collections;

public class LoadingManager : MonoBehaviour
{
    public static LoadingManager Instance { get; private set; }

    public float fadeDuration = 1f;
    public CanvasGroup canvasGroup;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else if (Instance != this)
        {
            Destroy(gameObject);
        }
    }

    public void LoadScene(string sceneName)
    {
        StartCoroutine(FadeTransition(sceneName));
    }

    private IEnumerator FadeTransition(string sceneName)
    {
        canvasGroup.alpha = 0f;
        canvasGroup.gameObject.SetActive(true);

        yield return canvasGroup.DOFade(1f, fadeDuration / 2f).WaitForCompletion();

        SceneManager.LoadScene(sceneName);
        yield return null;

        yield return canvasGroup.DOFade(0f, fadeDuration / 2f).WaitForCompletion();

        canvasGroup.gameObject.SetActive(false);
    }
}
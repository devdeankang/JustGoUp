using hardartcore.CasualGUI;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class MainMenuController : MonoBehaviour
{
    public CanvasChanger canvasChnager;
    public CanvasGroup mainMenuCanvasGroup;
    
    public void OnClickPlay()
    {
        StartCoroutine(TransitionToGame());
    }

    IEnumerator TransitionToGame()
    {
        canvasChnager.HideCanvas(mainMenuCanvasGroup);
        yield return new WaitForSeconds(0.6f);
        LoadingManager.Instance.LoadScene(SceneNames.Stage01);
    }
}

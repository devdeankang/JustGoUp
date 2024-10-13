using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    public GameObject obstaclePrefab;
    public Transform[] spawnPoints;    
    public GameObject menuUI;

    private List<GameObject> currentObstacles = new List<GameObject>();
    private bool isPaused = false;
    private int currentStage = 1;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;            
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void LoadStageObstacles(int stage)
    {
        ClearObstacles();

        string prefabPath = StageResourceManager.GetStagePrefabPath(stage);
        GameObject[] obstaclePrefabs = Resources.LoadAll<GameObject>(prefabPath);
                
        for (int i = 0; i < spawnPoints.Length; i++)
        {
            Transform spawnPoint = spawnPoints[i % spawnPoints.Length];
            GameObject prefab = obstaclePrefabs[i % obstaclePrefabs.Length];

            GameObject obstacle = Instantiate(prefab, spawnPoint.position, spawnPoint.rotation);
            currentObstacles.Add(obstacle);
        }

        currentStage = stage;
    }

    public void ClearObstacles()
    {
        foreach (var obstacle in currentObstacles)
        {
            Destroy(obstacle);
        }

        currentObstacles.Clear();
    }

    public void PauseGame()
    {
        if (!isPaused)
        {
            Time.timeScale = 0f;
            isPaused = true;
        }
    }

    public void ResumeGame()
    {
        if (isPaused)
        {
            Time.timeScale = 1f;
            isPaused = false;
        }
    }

    public void ResetStage()
    {
        ClearObstacles();
        LoadStageObstacles(currentStage);
    }

    private void ShowMenu()
    {
        menuUI.SetActive(true);
    }

    private void HideMenu()
    {
        menuUI.SetActive(false);
    }

    public void NextStage()
    {
        currentStage++;
        ResetStage();
    }
}
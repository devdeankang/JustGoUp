using System.Collections.Generic;
using UnityEngine;
using System.IO;

[System.Serializable]
public class StageData
{
    public int stage;
    public string path;
}

[System.Serializable]
public class StageDataList
{
    public List<StageData> stages;
}

public class StageResourceManager : MonoBehaviour
{
    private static StageDataList stageDataList;

    public static void LoadStageData()
    {
        string jsonPath = Path.Combine(Application.streamingAssetsPath, "stage_paths.json");
        if (File.Exists(jsonPath))
        {
            string jsonData = File.ReadAllText(jsonPath);
            stageDataList = JsonUtility.FromJson<StageDataList>(jsonData);
        }
        else
        {
            Debug.LogError("JSON file(Stage Data) not found");
        }
    }

    public static string GetStagePrefabPath(int stage)
    {
        if (stageDataList == null) LoadStageData();

        foreach (StageData data in stageDataList.stages)
        {
            if (data.stage == stage)
            {
                return data.path;
            }
        }

        Debug.LogError($"Invalid Stage Number: {stage}");
        return null;
    }
}
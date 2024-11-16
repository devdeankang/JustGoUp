using UnityEngine;

public static class LayerTagManager
{
    // LayerName
    public const string PlayerLayer = "Player";
    public const string CrawlingPlayerLayer = "CrawlingPlayer";
    public const string NonCrawlObstacleLayer = "NonCrawlObstacle";

    // TagName
    public const string UITag = "UI";
    public const string PlayerTag = "Player";
    public const string NonCrawlObstacleTag = "NonCrawlObstacle";


    public static void SetLayerRecursively(GameObject obj, int newLayer)
    {
        obj.layer = newLayer;

        foreach (Transform child in obj.transform)
        {
            SetLayerRecursively(child.gameObject, newLayer);
        }
    }
}

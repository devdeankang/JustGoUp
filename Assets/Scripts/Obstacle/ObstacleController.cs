using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.FullSerializer;
using UnityEngine;

public class ObstacleController : MonoBehaviour
{
    PlayerController player;
    public List<StrategyConfig> strategiesConfig;
    private List<IObstacleStrategy> strategies = new List<IObstacleStrategy>();

    void Start()
    {
        player = GameObject.FindWithTag(LayerTagManager.PlayerTag).GetComponent<PlayerController>();
        InitializeStrategies();
    }

    void Update()
    {
        ExecuteStrategies();
    }

    public void InitializeStrategies()
    {
        foreach (var config in strategiesConfig)
        {
            if (config == null) continue;

            if (config is PushConfig pushConfig)
            {
                if (pushConfig.pushPrefab == null)
                {
                    Debug.LogError("PushConfig의 pushPrefab이 설정되지 않음.");
                    continue;
                }
            }

            var strategy = StrategyFactory.CreateStrategy(config);
            if (strategy != null)
            {
                strategies.Add(strategy);
            }
        }
    }

    private void ExecuteStrategies()
    {
        foreach (var strategy in strategies)
        {
            strategy.Execute(this);
        }
    }

    private void OnCollisionEnter(Collision coll)
    {
        if (coll.gameObject.CompareTag(LayerTagManager.PlayerTag))
        {   
            if (player != null)
            {
                foreach (var strategy in strategies)
                {
                    if (strategy is ICollisionStrategy collisionStrategy)
                    {
                        collisionStrategy.ExecuteCollisionResponse(this, coll, player.GetComponent<Rigidbody>());
                    }
                }
            }
        }
    }

    public void HandleSubCollision(Collision coll, GameObject subObject)
    {
        if(coll.gameObject.CompareTag(LayerTagManager.PlayerTag))
        {
            foreach(var strategy in strategies)
            {
                if(strategy is ICollisionStrategy collisionStrategy)
                {
                    collisionStrategy.ExecuteCollisionResponse(this, coll, player.GetComponent<Rigidbody>());
                }
            }
        }
    }

    public void HandleCollisionWithPlayer(float damage, float knockbackForce, Vector3 collisionPoint)
    {   
        if (player != null)
        {
            player.ApplyHit(damage, knockbackForce, collisionPoint);
        }
    }

    public void LoadStageStrategies(string stageName)
    {
        strategies.Clear();

        var configs = Resources.LoadAll<StrategyConfig>($"ObstacleConfigs/{stageName}"); // ##
        foreach (var config in configs)
        {
            var strategy = StrategyFactory.CreateStrategy(config);
            if (strategy != null)
            {
                strategies.Add(strategy);
            }
        }
    }

    public void AddTimeLimitedStrategy(StrategyConfig innerConfig, float timeLimit)
    {
        TimeLimitedConfig timeLimitedConfig = StrategyFactory.CreateTimeLimitedConfig(innerConfig, timeLimit);
        strategiesConfig.Add(timeLimitedConfig);
        var strategy = StrategyFactory.CreateStrategy(timeLimitedConfig);
        if (strategy != null)
        {
            strategies.Add(strategy);
        }
    }
    
    public void SpawnPushedObstacle(GameObject prefab)
    {
        foreach (Transform spawnPoint in transform.GetComponentsInChildren<Transform>())
        {
            if (spawnPoint == transform) continue;

            GameObject obj = PoolManager.Instance.GetFromPool(prefab, spawnPoint.position, Quaternion.identity);

            var controller = obj.GetComponent<ObstacleController>();
            controller?.InitializeStrategies();
        }
    }

}
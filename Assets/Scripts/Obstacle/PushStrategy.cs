using System.Collections;
using UnityEditor.EditorTools;
using UnityEngine;

public class PushStrategy : IObstacleStrategy
{
    PushConfig config;
    bool isSpawning;

    public PushStrategy(StrategyConfig config)
    {
        this.config = config as PushConfig;
    }

    public void Execute(ObstacleController obstacle, bool hasAnimator = false)
    {
        if (config == null || config.pushPrefab == null) return;

        if (!isSpawning)
        {
            obstacle.StartCoroutine(SpawnObstacleRoutine(obstacle));
        }
    }

    private IEnumerator SpawnObstacleRoutine(ObstacleController obstacle)
    {
        isSpawning = true;

        while (true)
        {
            SpawnPushedObstacle(obstacle);            
            yield return new WaitForSeconds(config.spawnInterval);
        }
    }
    
    private void SpawnPushedObstacle(ObstacleController obstacle)
    {        
        foreach (Transform spawnPoint in obstacle.transform)
        {
            if (spawnPoint == obstacle.transform) continue;
            
            GameObject obj = PoolManager.Instance.GetFromPool(config.pushPrefab, spawnPoint.position, config.pushPrefab.transform.rotation);
            obj.transform.parent = spawnPoint;

            Rigidbody rb = obj.GetComponent<Rigidbody>();
            if (rb == null)
            {
                Debug.LogError($"PushPrefab {config.pushPrefab.name}에 Rigidbody가 없습니다.");
                return;
            }
            rb.isKinematic = false;
            rb.useGravity = true;
                
            Vector3 forceDirection = config.pushDirection.normalized;
            forceDirection.y = 0f;
            rb.AddForce(forceDirection * config.pushSpeed, ForceMode.VelocityChange);

            obstacle.StartCoroutine(HandleLifeTime(obj));
        }
    }

    private IEnumerator HandleLifeTime(GameObject obj)
    {
        yield return new WaitForSeconds(config.lifeTime);

        PoolManager.Instance.ReturnToPool(obj);
    }
}
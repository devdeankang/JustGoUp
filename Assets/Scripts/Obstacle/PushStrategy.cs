using UnityEngine;

public class PushStrategy : IObstacleStrategy, ICollisionStrategy
{
    PushConfig config;
    
    public PushStrategy(StrategyConfig config)
    {
        this.config = config as PushConfig;
    }

    public void Execute(ObstacleController obstacle)
    {
        if (config == null) return;

        Rigidbody rb = obstacle.GetComponent<Rigidbody>();
        if (rb == null)
        {
            rb = obstacle.gameObject.AddComponent<Rigidbody>();
        }

        Vector3 direction = config.pushDirection;

        var player = GameObject.FindWithTag(LayerTagManager.PlayerTag);
        if (player != null)
        {
            direction = (player.transform.position - obstacle.transform.position).normalized;
        }

        rb.MovePosition(obstacle.transform.position + direction * config.pushSpeed * Time.deltaTime);
    }

    public void ExecuteCollisionResponse(Collision coll, Rigidbody target_rb)
    {
        if (config == null) return;

        Vector3 forceDirection = config.pushDirection;

        if (forceDirection == Vector3.zero)
        {
            Vector3 contactPoint = coll.contacts[0].point;
            forceDirection = (target_rb.transform.position - contactPoint).normalized;
        }

        target_rb.AddForce(forceDirection * config.pushForce, ForceMode.Impulse);
    }
}
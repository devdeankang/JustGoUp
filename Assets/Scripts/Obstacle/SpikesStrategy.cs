using UnityEngine;

public class SpikesStrategy : IObstacleStrategy, ICollisionStrategy
{
    SpikesConfig config;
    Vector3 initialPosition;
    Vector3 targetPosition;
    bool isRising;
    bool isFalling;
    float timer;

    public SpikesStrategy(StrategyConfig config)
    {
        this.config = config as SpikesConfig;
        timer = 0f;
        isRising = false;
        isFalling = false;
    }

    public void Execute(ObstacleController obstacle)
    {
        if (config == null) return;

        timer += Time.deltaTime;
        if (timer < config.spikeInterval) return;

        if (!isRising && !isFalling)
        {
            targetPosition = initialPosition + new Vector3(0f, config.spikeHeight, 0f);
            isRising = true;
        }

        if (isRising)
        {
            obstacle.transform.position = Vector3.MoveTowards(obstacle.transform.position, targetPosition, config.riseSpeed * Time.deltaTime);
            if (Vector3.Distance(obstacle.transform.position, targetPosition) < 0.01f)
            {
                isRising = false;
                isFalling = true;
                timer = 0f;
            }
        }

        if (isFalling)
        {
            obstacle.transform.position = Vector3.MoveTowards(obstacle.transform.position, initialPosition, config.fallSpeed * Time.deltaTime);
            if (Vector3.Distance(obstacle.transform.position, initialPosition) < 0.01f)
            {
                isFalling = false;
                timer = 0f;
            }
        }
    }

    public void ExecuteCollisionResponse(Collision coll, Rigidbody target_rb)
    {
        if (config == null) return;

        Vector3 contactPoint = coll.contacts[0].point;
        Vector3 direction = (target_rb.transform.position - contactPoint).normalized;
        Vector3 knockbackDir = Quaternion.Euler(0, config.knockbackAngle, 0) * direction;
        target_rb.AddForce(knockbackDir * config.knockbackForce, ForceMode.Impulse);
    }

    public void SetInitialPosition(Vector3 position)
    {
        initialPosition = position;
    }
}
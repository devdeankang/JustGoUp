using UnityEngine;

public class SwingStrategy : IObstacleStrategy, ICollisionStrategy
{
    SwingConfig config;

    public SwingStrategy(StrategyConfig config)
    {
        this.config = config as SwingConfig;
    }

    public void Execute(ObstacleController obstacle)
    {
        if (config == null) return;

        float angle = Mathf.Sin(Time.time * config.swingSpeed) * config.swingAngle;
        obstacle.transform.rotation = Quaternion.Euler(0, 0, angle);
    }

    public void ExecuteCollisionResponse(Collision coll, Rigidbody target_rb)
    {
        if (config == null) return;

        Vector3 contactPoint = coll.contacts[0].point;
        Vector3 swingDirection = (target_rb.transform.position - contactPoint).normalized;
        Vector3 knockbackDirection = Quaternion.Euler(0, config.swingAngle, 0) * swingDirection;
        target_rb.AddForce(knockbackDirection * config.swingForce, ForceMode.Impulse);
    }
}
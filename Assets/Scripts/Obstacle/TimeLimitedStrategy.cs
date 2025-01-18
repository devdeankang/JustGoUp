using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeLimitedStrategy : IObstacleStrategy, ICollisionStrategy
{
    private IObstacleStrategy innerStrategy;
    private float timeLimit;
    private float elapsedTime;

    public TimeLimitedStrategy(IObstacleStrategy innerStrategy, float timeLimit)
    {
        this.innerStrategy = innerStrategy;
        this.timeLimit = timeLimit;
        elapsedTime = 0f;
    }

    public void Execute(ObstacleController obstacle, bool hasAnimator = true)
    {
        // unlimited
        if (timeLimit < 0f)
        {
            innerStrategy.Execute(obstacle, hasAnimator);
            return;
        }

        if (elapsedTime < timeLimit)
        {
            innerStrategy.Execute(obstacle, hasAnimator);
            elapsedTime += Time.deltaTime;
        }
    }
    
    public void ExecuteCollisionResponse(ObstacleController obstacle, Collision coll, Rigidbody targetRb)
    {
        if (innerStrategy is ICollisionStrategy collisionStrategy)
        {
            collisionStrategy.ExecuteCollisionResponse(obstacle, coll, targetRb);
        }
    }

    public void ResetElapsedTime()
    {
        elapsedTime = 0f;
    }
}

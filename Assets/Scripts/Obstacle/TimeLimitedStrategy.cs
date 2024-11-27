using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeLimitedStrategy : IObstacleStrategy
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

    public void Execute(ObstacleController obstacle)
    {
        // unlimited
        if (timeLimit < 0f)
        {
            innerStrategy.Execute(obstacle);
            return;
        }

        if (elapsedTime < timeLimit)
        {
            innerStrategy.Execute(obstacle);
            elapsedTime += Time.deltaTime;
        }
    }

    public void ResetElapsedTime()
    {
        elapsedTime = 0f;
    }
}

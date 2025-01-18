using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PriorityCompositeStrategy : IObstacleStrategy
{
    private List<(IObstacleStrategy strategy, int priority)> strategies = new List<(IObstacleStrategy, int)>();

    public void AddStrategy(IObstacleStrategy strategy, int priority)
    {
        strategies.Add((strategy, priority));
        strategies.Sort((a, b) => a.priority.CompareTo(b.priority));
    }

    public void Execute(ObstacleController obstacle, bool hasAnimator = true)
    {
        foreach (var (strategy, _) in strategies)
        {
            strategy.Execute(obstacle);
        }
    }
}

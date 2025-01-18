using UnityEngine;

public interface IObstacleStrategy
{
    void Execute(ObstacleController obstacle, bool hasAnimator = true);
}

public interface ICollisionStrategy
{
    void ExecuteCollisionResponse(ObstacleController obstacle, Collision coll, Rigidbody target_rb = null);
}
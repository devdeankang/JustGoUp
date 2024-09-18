using UnityEngine;

public interface IObstacleStrategy
{
    void Execute(ObstacleController obstacle);
}

public interface ICollisionStrategy
{
    void ExecuteCollisionResponse(Collision coll, Rigidbody target_rb);
}
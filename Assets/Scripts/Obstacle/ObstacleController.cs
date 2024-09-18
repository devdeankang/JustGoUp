using UnityEngine;

public class ObstacleController : MonoBehaviour
{
    private IObstacleStrategy strategy;
    private ICollisionStrategy collisionStrategy;
    public bool isActive = false;    

    void Start()
    {
        if (isActive && strategy != null)
        {
            ExecuteStrategy();
        }
    }

    public void SetStrategy(IObstacleStrategy newStrategy)
    {
        strategy = newStrategy;
    }

    public void SetCollisionStrategy(ICollisionStrategy newCollisionStrategy)
    {
        collisionStrategy = newCollisionStrategy;
    }

    public void ExecuteStrategy()
    {
        if (strategy != null)
        {
            strategy.Execute(this);
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collisionStrategy != null)
        {            
            collisionStrategy.ExecuteCollisionResponse(collision, collision.rigidbody);
        }
    }

    public void ActivateObstacle()
    {
        isActive = true;
        ExecuteStrategy();
    }

    public void DeactivateObstacle()
    {
        isActive = false;
    }

    public void ResetObstacle()
    {
        transform.position = Vector3.zero;
    }
}

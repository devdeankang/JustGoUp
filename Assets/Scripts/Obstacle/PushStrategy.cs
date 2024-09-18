using UnityEngine;

public class PushStrategy : IObstacleStrategy, ICollisionStrategy
{
    float pushSpeed;
    float pushForce;
    Vector3 pushDirection;

    public PushStrategy(float pushSpeed, Vector3 pushDirection, float pushForce = 1f)
    {
        this.pushForce = pushForce;
        this.pushSpeed = pushSpeed;
        this.pushDirection = pushDirection;
    }

    public void Execute(ObstacleController obstacle)
    {
        Rigidbody rb = obstacle.GetComponent<Rigidbody>();
        if (rb == null)
        {
            rb = obstacle.gameObject.AddComponent<Rigidbody>();
        }

        rb.MovePosition(obstacle.transform.position + pushDirection * pushSpeed * Time.deltaTime);

        obstacle.transform.Translate(pushDirection * pushSpeed * Time.deltaTime);
    }
    
    public void ExecuteCollisionResponse(Collision coll, Rigidbody target_rb)
    {
        Vector3 forceDirection = coll.gameObject.transform.position - coll.collider.transform.position;
        forceDirection.y = 0f;

        target_rb.AddForce(forceDirection.normalized * pushForce, ForceMode.Impulse);
    }

}
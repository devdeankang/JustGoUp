using UnityEngine;

public class SwingStrategy : IObstacleStrategy, ICollisionStrategy
{
    float swingSpeed;
    float swingAngle;
    float swingForce;

    public SwingStrategy(float swingForce = 1f, float swingSpeed = 1f, float swingAngle = 30f)
    {
        this.swingForce = swingForce;
        this.swingSpeed = swingSpeed;
        this.swingAngle = swingAngle;
    }

    public void Execute(ObstacleController obstacle)
    {
        float angle = Mathf.Sin(Time.time * swingSpeed) * swingAngle;
        obstacle.transform.rotation = Quaternion.Euler(0, 0, angle);
    }

    public void ExecuteCollisionResponse(Collision coll, Rigidbody target_rb)
    {
        Vector3 contactPoint = coll.contacts[0].point;
        Vector3 swingDirection = (target_rb.transform.position - contactPoint).normalized;

        Vector3 knockbackDirection = Quaternion.Euler(0, swingAngle, 0) * swingDirection;
        target_rb.AddForce(knockbackDirection * swingForce, ForceMode.Impulse);
    }
}
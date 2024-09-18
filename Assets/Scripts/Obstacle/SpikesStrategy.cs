using UnityEngine;

public class SpikesStrategy : IObstacleStrategy, ICollisionStrategy
{
    float spikeInterval;
    float timer;
    Vector3 initialPosition;
    Vector3 targetPosition;
    float spikeHeight;
    float riseSpeed;
    float fallSpeed;
    bool isRising;
    bool isFalling;

    float knockbackForce;
    float knockbackAngle;

    public SpikesStrategy(float spikeInterval, float spikeHeight, float riseSpeed, float fallSpeed, float knockbackForce, float knockbackAngle)
    {
        this.timer = 0f;
        this.spikeInterval = spikeInterval;
        this.spikeHeight = spikeHeight;
        this.riseSpeed = riseSpeed;
        this.fallSpeed = fallSpeed;
        this.isRising = false;
        this.isFalling = false;

        this.knockbackForce = knockbackForce;
        this.knockbackAngle = knockbackAngle;
    }

    public void Execute(ObstacleController obstacle)
    {
        timer += Time.deltaTime;
        if (timer < spikeInterval) return;

        if (!isRising && !isFalling)
        {
            targetPosition = initialPosition + new Vector3(0f, spikeHeight, 0f);
            isRising = true;
        }

        if (isRising)
        {
            obstacle.transform.position = Vector3.MoveTowards(obstacle.transform.position, targetPosition, riseSpeed * Time.deltaTime);

            if (Vector3.Distance(obstacle.transform.position, targetPosition) < 0.01f)
            {
                isRising = false;
                isFalling = true;
                timer = 0f;
            }
        }

        if (isFalling)
        {
            obstacle.transform.position = Vector3.MoveTowards(obstacle.transform.position, initialPosition, fallSpeed * Time.deltaTime);

            if (Vector3.Distance(obstacle.transform.position, initialPosition) < 0.01f)
            {
                isFalling = false;
                timer = 0f;
            }
        }
    }

    public void ExecuteCollisionResponse(Collision coll, Rigidbody target_rb)
    {
        Vector3 contactPoint = coll.contacts[0].point;
        Vector3 direction = (target_rb.transform.position - contactPoint).normalized;

        Vector3 knockbackDir = Quaternion.Euler(0, knockbackAngle, 0) * direction;
        target_rb.AddForce(knockbackDir * knockbackForce, ForceMode.Impulse);
    }

    public void SetInitialPosition(Vector3 position)
    {
        initialPosition = position;
    }
}
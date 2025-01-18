using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurningGrinderStrategy : IObstacleStrategy, ICollisionStrategy
{
    Animator _animator;
    TurningGrinderConfig _config;
    float currentAngle;    
    
    public TurningGrinderStrategy(StrategyConfig config)
    {
        this._config = config as TurningGrinderConfig;
        currentAngle = 0f;
    }

    public void Execute(ObstacleController obstacle, bool hasAnimator = true)
    {
        if (_config == null) return;
        if (_animator == null) _animator = obstacle.GetComponent<Animator>();

        if (hasAnimator)
        {
            _animator.speed = _config.turningSpeed;

            AnimatorStateInfo stateInfo = _animator.GetCurrentAnimatorStateInfo(0);
            if (!stateInfo.IsName(AnimationStates.TurningGrinder))
            {
                _animator.Play(AnimationStates.TurningGrinder, 0, 0f);
            }
        }
        else
        {
            currentAngle += _config.turningSpeed * Time.deltaTime;
            if (currentAngle >= 360f) currentAngle -= 360f;

            Vector3 offset = Quaternion.Euler(_config.rotationAxis * currentAngle) *
                             (Vector3.right * _config.rotationRadius);
            obstacle.transform.position = obstacle.transform.parent.position + offset;

            obstacle.transform.Rotate(_config.rotationAxis * _config.turningSpeed * Time.deltaTime);
        }       
    }

    public void ExecuteCollisionResponse(ObstacleController obstacle, Collision coll, Rigidbody targetRb)
    {
        if (coll.gameObject.CompareTag(LayerTagManager.PlayerTag))
        {
            Vector3 contactPoint = coll.contacts[0].point;

            if (_animator != null)
            {
                AnimatorStateInfo stateInfo = _animator.GetCurrentAnimatorStateInfo(0);

                float progress = stateInfo.normalizedTime % 1.0f;
                Vector3 knockbackDirection = CalculateKnockbackDirectionFromAnimation(obstacle.transform, contactPoint, progress);
                obstacle.HandleCollisionWithPlayer(_config.damageValue, _config.knockbackForce, knockbackDirection);
            }
            else
            {
                Vector3 knockbackDirection = (coll.gameObject.transform.position - contactPoint).normalized;
                obstacle.HandleCollisionWithPlayer(_config.damageValue, _config.knockbackForce, knockbackDirection);
            }
        }
    }

    Vector3 CalculateKnockbackDirectionFromAnimation(Transform obstacleTransform, Vector3 contactPoint, float animationProgress)
    {
        Vector3 direction = Quaternion.Euler(0, animationProgress * 360f, 0) * Vector3.forward;

        Vector3 toContactPoint = (contactPoint - obstacleTransform.position).normalized;
        Vector3 knockbackDirection = Vector3.Cross(direction, toContactPoint).normalized;

        return knockbackDirection;
    }
}

using System;
using UnityEditor;
using UnityEngine;

public class SpikesStrategy : IObstacleStrategy, ICollisionStrategy
{
    SpikesConfig _config;
    Animator _animator;
    Vector3 initialPosition;
    Vector3 targetPosition;
    bool isRising;
    bool isFalling;
    float timer;
    bool isWaiting;

    public SpikesStrategy(StrategyConfig config)
    {
        this._config = config as SpikesConfig;        
        timer = 0f;
        isRising = false;
        isFalling = false;
        isWaiting = false;
    }

    public void Execute(ObstacleController obstacle, bool hasAnimator = true)
    {
        if (_config == null) return;
        if (_animator == null) _animator = obstacle.GetComponent<Animator>();

        if (hasAnimator)
        {           
            AnimatorStateInfo stateInfo = _animator.GetCurrentAnimatorStateInfo(0);

            if (isWaiting)
            {
                timer += Time.deltaTime;
                if (timer >= _config.spikeInterval)
                {
                    timer = 0f;
                    isWaiting = false;
                    _animator.Play(AnimationStates.Spike, 0, 0f);
                }
            }
            else
            {
                if (stateInfo.normalizedTime >= 1.0f && !stateInfo.loop)
                {
                    isWaiting = true;
                }
            }
        }
        else
        {
            timer += Time.deltaTime;
            if (timer < _config.spikeInterval) return;

            if (!isRising && !isFalling)
            {
                targetPosition = initialPosition + new Vector3(0f, _config.spikeHeight, 0f);
                isRising = true;
            }

            if (isRising)
            {
                obstacle.transform.position = Vector3.MoveTowards(obstacle.transform.position, targetPosition, _config.riseSpeed * Time.deltaTime);
                if (Vector3.Distance(obstacle.transform.position, targetPosition) < 0.01f)
                {
                    isRising = false;
                    isFalling = true;
                    timer = 0f;
                }
            }

            if (isFalling)
            {
                obstacle.transform.position = Vector3.MoveTowards(obstacle.transform.position, initialPosition, _config.fallSpeed * Time.deltaTime);
                if (Vector3.Distance(obstacle.transform.position, initialPosition) < 0.01f)
                {
                    isFalling = false;
                    timer = 0f;
                }
            }
        }
    }

    public void ExecuteCollisionResponse(ObstacleController obstacle, Collision coll, Rigidbody target_rb)
    {
        if (coll.gameObject.CompareTag(LayerTagManager.PlayerTag))
        {
            obstacle.HandleCollisionWithPlayer(_config.damageValue, _config.knockbackForce, coll.contacts[0].point);
        }
    }

    public void SetInitialPosition(Vector3 position)
    {
        initialPosition = position;
    }
}
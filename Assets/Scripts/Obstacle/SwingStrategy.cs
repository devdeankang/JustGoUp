using UnityEngine;

public class SwingStrategy : IObstacleStrategy, ICollisionStrategy
{
    private SwingConfig _config;
    private Animator _animator;    

    public SwingStrategy(StrategyConfig config)
    {
        this._config = config as SwingConfig;
    }

    public void Execute(ObstacleController obstacle, bool hasAnimator = true)
    {
        if (_config == null) return;
        if(_animator == null) _animator = obstacle.GetComponent<Animator>();
        
        if (hasAnimator)
        {
            _animator.speed = _config.swingSpeed;
            AnimatorStateInfo stateInfo = _animator.GetCurrentAnimatorStateInfo(0);

            if (!stateInfo.IsName(AnimationStates.Swing))
            {
                _animator.Play(AnimationStates.Swing, 0, 0f);
            }
        }
        else
        {
            float angle = Mathf.Sin(Time.time * _config.swingSpeed) * _config.swingAngle;
            obstacle.transform.rotation = Quaternion.Euler(0, 0, angle);
        }
    }

    public void ExecuteCollisionResponse(ObstacleController obstacle, Collision coll, Rigidbody targetRb)
    {
        if (_config == null) return;

        if (coll.gameObject.CompareTag(LayerTagManager.PlayerTag))
        {
            Vector3 contactPoint = coll.contacts[0].point;
            Vector3 knockbackDirection = (targetRb.transform.position - contactPoint).normalized;
            
            obstacle.HandleCollisionWithPlayer(_config.damageValue, _config.swingForce, knockbackDirection);
        }
    }
}
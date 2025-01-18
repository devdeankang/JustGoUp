using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal.Internal;

public class HitState : State<PlayerController>
{
    PlayerController player;
    float damage;
    float knockbackForce;
    Vector3 collisionPoint;

    public override void Enter(PlayerController player)
    {
        this.player = player;
        if (player.isInvincible) return;

        player.TakeDamage(damage);
        player.StartInvincibility();

        Rigidbody rb = player.GetComponent<Rigidbody>();
        if (rb != null)
        {
            Vector3 knockbackDirection = (player.transform.position - collisionPoint).normalized;
            rb.AddForce(knockbackDirection * knockbackForce, ForceMode.Impulse);
        }
        
        if (player.cameraController != null)
        {
            player.cameraController.ShakeCamera(0.1f, 0.15f);                        
        }        
    }

    public override void Update(PlayerController player)
    {
        HandleChangeState(this.player);
    }

    public override void FixedUpdate(PlayerController player)
    {

    }

    public override void Exit(PlayerController player)
    {

    }
        
    public void SetHitData(float damage, float knockbackForce, Vector3 collisionPoint)
    {
        this.damage = damage;
        this.knockbackForce = knockbackForce;
        this.collisionPoint = collisionPoint;
    }
}
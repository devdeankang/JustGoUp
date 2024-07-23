using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class JumpState : State<PlayerController>
{
    public override void Enter(PlayerController player)
    {
        
    }

    public override void Update(PlayerController player)
    {
        HandleChangeState(player);
    }

    public override void FixedUpdate(PlayerController player)
    {       
        if (player.IsJump && player.isGrounded && player.anim.GetInteger("up") == 4 && player.IsActive)
        {
            player.coll.material.dynamicFriction = 0;
            player.coll.material.staticFriction = 0;
            if (player.anim.GetFloat("walk") <= 0f)
            {
                player.anim.Play("jump");
            }
            if (player.anim.GetFloat("walk") > 0f)
            {
                player.anim.Play("runjumpin");
            }

            player.IsJump = false;
        }
        if (player.IsJump && player.isGrounded && player.anim.GetInteger("up") == 3 && player.IsActive)
        {
            if (player.anim.GetFloat("walk") == 0f) player.StartCoroutine("Wait", player.waitTime);
            player.rayCorrection = 0.025f;
            player.anim.SetInteger("up", 4);
            player.tr.rotation = Quaternion.LookRotation(-new Vector3(player.PlayerForce.x, 0f, player.PlayerForce.z), Vector3.up);

            player.IsJump = false;
        }
    }

    public override void Exit(PlayerController player)
    {
        
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class JumpState : State<PlayerController>
{
    PlayerController player;    

    public override void Enter(PlayerController player)
    {
        this.player = player;
        player.IsJump = true;        
        ExecuteJump();        
    }

    public override void Update(PlayerController player)
    {
        if(player.isGrounded)
        {
            HandleChangeState(player);
        }
    }

    public override void FixedUpdate(PlayerController player)
    {
        
    }

    public override void Exit(PlayerController player)
    {
        
    }

    private void ExecuteJump()
    {
        if (player.isGrounded && player.anim.GetInteger("up") == 4 && player.IsActive)
        {   
            if (player.anim.GetFloat("walk") <= 0f)
            {
                player.anim.Play("jump");
            }
            else
            {
                player.anim.Play("runjumpin");
            }
        }
        if (player.isGrounded && player.anim.GetInteger("up") == 3 && player.IsActive)
        {
            player.rayCorrection = 0.025f;
            player.anim.SetInteger("up", 4);
            player.tr.rotation = Quaternion.LookRotation(new Vector3(player.PlayerForce.x, 0f, player.PlayerForce.z), Vector3.up);
        }

        player.IsJump = false;
    }       

}

using UnityEngine;

public class FallState : State<PlayerController>
{
    public override void Enter(PlayerController player)
    {
        //player.anim.SetTrigger("Fall");
    }

    public override void Update(PlayerController player)
    {
        if (player.isGrounded)
        {
            player.stateMachine.ChangeState(player.stateMap[PlayerController.State.Idle]);
        }
    }

    public override void FixedUpdate(PlayerController player)
    {
        // Add logic for fall movement if needed

        
    }

    public override void Exit(PlayerController player)
    {
        //player.anim.ResetTrigger("Fall");
    }
}
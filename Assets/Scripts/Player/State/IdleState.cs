using UnityEngine;

public class IdleState : State<PlayerController>
{
    public override void Enter(PlayerController player)
    {
        player.anim.SetBool("grounded", player.isGrounded);        
        player.anim.SetFloat("walk", 0);

        if (player.cameraController != null)
        {            
            Vector3 cameraOffset = player.transform.rotation * new Vector3(0, 8.79f, -15.57f);            
            player.cameraController.SetIdleCameraOffset(cameraOffset);
        }
    }

    public override void Update(PlayerController player)
    {
        HandleChangeState(player);
    }

    public override void FixedUpdate(PlayerController player)
    {
        
    }

    public override void Exit(PlayerController player)
    {

    }
}

using UnityEngine;

public class IdleState : State<PlayerController>
{
    //public Vector3 cameraOffset = new Vector3(0, 1, -2);
    //public Vector3 cameraRotationOffset = new Vector3(-10, 0, 0);

    public override void Enter(PlayerController player)
    {
        player.anim.SetBool("grounded", player.isGrounded);        
        player.anim.SetFloat("walk", 0);
                
        //if (player.cameraController != null)
        //{
        //    player.cameraController.SetIdleCameraOffset(cameraOffset, cameraRotationOffset);            
        //}
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
        //// 카메라 오프셋을 기본값으로 되돌림
        //if (player.cameraController != null)
        //{
        //    player.cameraController.ResetCameraOffset();
        //}
    }
}

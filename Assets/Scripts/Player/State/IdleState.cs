using UnityEngine;
using UnityEngine.Rendering.Universal;

public class IdleState : State<PlayerController>
{
    bool hasSwitchedView;
    float cameraTransitionThresholds = 3.5f;
    float timer;

    public override void Enter(PlayerController player)
    {
        timer = 0f;
        hasSwitchedView = false;
        player.anim.SetBool("grounded", player.isGrounded);        
        player.anim.SetFloat("walk", 0);
    }

    public override void Update(PlayerController player)
    {        
        timer += Time.deltaTime;        
        if (player.cameraController != null && timer >= cameraTransitionThresholds && !hasSwitchedView)
        {            
            Vector3 cameraOffset = player.transform.rotation * new Vector3(0, 8.79f, -15.57f);
            player.cameraController.SetIdleCameraOffset(cameraOffset);
            hasSwitchedView = true;
        }

        HandleChangeState(player);
    }

    public override void FixedUpdate(PlayerController player)
    {
        
    }

    public override void Exit(PlayerController player)
    {

    }
}

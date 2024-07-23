using UnityEngine;

public class IdleState : State<PlayerController>
{
    public Vector3 cameraOffset = new Vector3(0, 1, -2); // ī�޶��� ������ ����
    public Vector3 cameraRotationOffset = new Vector3(-10, 0, 0);

    public override void Enter(PlayerController player)
    {
        player.anim.SetInteger("up", 4);
        player.anim.SetFloat("walk", 0);

        // ī�޶��� ��ġ�� ĳ���� �ڷ� ����
        if (player.cameraController != null)
        {
            player.cameraController.SetIdleCameraOffset(cameraOffset, cameraRotationOffset);            
        }
    }

    public override void Update(PlayerController player)
    {
        HandleChangeState(player);
    }

    public override void FixedUpdate(PlayerController player)
    {
        // Ground check logic moved here if needed specifically for Idle
    }

    public override void Exit(PlayerController player)
    {        
        // ī�޶� �������� �⺻������ �ǵ���
        if (player.cameraController != null)
        {
            player.cameraController.ResetCameraOffset();
        }
    }
}

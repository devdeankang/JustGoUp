using UnityEngine;

public class MoveState : State<PlayerController>
{
    public override void Enter(PlayerController player)
    {
    }

    public override void Update(PlayerController player)
    {
        HandleChangeState(player);
        HandleRunState(player);
        UpdateMoveSpeed(player);
    }

    public override void FixedUpdate(PlayerController player)
    {
        player.isGrounded = Physics.Raycast(player.tr.position, Vector3.down, 0.1f);
        HandleMovement(player);
        if (!player.isMobileMode)
        {
            HandleKeyboardMovement(player);
        }
    }

    public override void Exit(PlayerController player)
    {
    }

    private void HandleRunState(PlayerController player)
    {
        if (player.RunButtonHoldTimer > 0)
        {
            player.RunButtonHoldTimer -= Time.deltaTime;
            if (player.RunButtonHoldTimer <= 0)
            {
                player.IsRun = false;
            }
        }

        if ((Input.GetKey(KeyCode.LeftShift) || player.IsRun) && player.RunAnimSpeed < 30)
        {
            player.RunAnimSpeed++;
            player.anim.SetFloat("Speed", 1f);
        }
        else if ((!Input.GetKey(KeyCode.LeftShift) || !player.IsRun) && player.RunAnimSpeed > 0)
        {
            player.RunAnimSpeed--;
            player.anim.SetFloat("Speed", 0.5f);
        }
    }

    private void UpdateMoveSpeed(PlayerController player)
    {
        player.moveSpeed = player.IsRun ? player.runSpeed : player.walkSpeed;
    }

    private void HandleMovement(PlayerController player)
    {
        Vector3 moveDirection = new Vector3(player.PlayerForce.x, 0, player.PlayerForce.z) * player.moveSpeed;
        player.rb.velocity = new Vector3(moveDirection.x, player.rb.velocity.y, moveDirection.z);
        player.PlayerForce = player.rb.velocity;
        player.anim.SetFloat("walk", player.PlayerForce.magnitude);
        player.anim.SetFloat("run", player.RunAnimSpeed);
    }

    private void HandleKeyboardMovement(PlayerController player)
    {
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");
        Vector3 keyboardMoveDir = new Vector3(x, 0, z) * player.moveSpeed;
        player.rb.velocity = new Vector3(keyboardMoveDir.x, player.rb.velocity.y, keyboardMoveDir.z);
        player.PlayerForce = player.rb.velocity;
        player.anim.SetFloat("walk", player.PlayerForce.magnitude);
        player.anim.SetFloat("run", player.RunAnimSpeed);
    }
}

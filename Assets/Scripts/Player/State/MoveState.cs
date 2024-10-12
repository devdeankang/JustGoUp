using UnityEngine;

public class MoveState : State<PlayerController>
{
    PlayerController player;

    public override void Enter(PlayerController player)
    {
        this.player = player;        
    }

    public override void Update(PlayerController player)
    {
        HandleChangeState(player);
        HandleRunState();        
    }

    public override void FixedUpdate(PlayerController player)
    {
        player.isGrounded = Physics.Raycast(player.tr.position, Vector3.down, 0.1f);
        HandleMovement();
    }

    public override void Exit(PlayerController player)
    {
        
    }

    private void HandleRunState()
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

    private void HandleMovement()
    {
        Vector3 moveDirection = new Vector3(player.PlayerForce.x, 0, player.PlayerForce.z) * player.moveSpeed;
        player.rb.velocity = new Vector3(moveDirection.x, player.rb.velocity.y, moveDirection.z);
        player.PlayerForce = player.rb.velocity;
        player.anim.SetFloat("walk", player.PlayerForce.magnitude);
        player.anim.SetFloat("run", player.RunAnimSpeed);
    }
}

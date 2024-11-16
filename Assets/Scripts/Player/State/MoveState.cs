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
        }
        else if ((!Input.GetKey(KeyCode.LeftShift) || !player.IsRun) && player.RunAnimSpeed > 0)
        {
            player.RunAnimSpeed--;            
        }
    }

    private void HandleMovement()
    {
        Vector3 moveDirection = new Vector3(player.PlayerForce.x, 0, player.PlayerForce.z).normalized;        
        player.rb.velocity = moveDirection * (player.anim.GetFloat("speed") + player.moveSpeed) + (new Vector3(0, player.rb.velocity.y, 0));
        player.PlayerForce = player.rb.velocity; //
        player.anim.SetFloat("walk", moveDirection.magnitude);
        player.anim.SetFloat("run", player.RunAnimSpeed);
    }
}

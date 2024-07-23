using UnityEngine;

public interface IState<T>
{
    void Enter(T sender);
    void Update(T sender);
    void FixedUpdate(T sender);
    void Exit(T sender);
    string GetName();
}


public abstract class State<T> : IState<T> where T : class
{
    public abstract void Enter(T player);
    public abstract void Update(T player);
    public abstract void FixedUpdate(T player);
    public abstract void Exit(T player);

    public string GetName()
    {
        return this.GetType().Name;
    }

    public void HandleChangeState(PlayerController player)
    {
        // Change MoveState        
        if ((Input.GetAxis("Vertical") != 0 || Input.GetAxis("Horizontal") != 0) ||
        (player.PlayerForce.x != 0 || player.PlayerForce.z != 0))
        {
            player.stateMachine.ChangeState(player.stateMap[PlayerController.State.Move]);
        }
        else
        {
            // Change IdleState
            player.stateMachine.ChangeState(player.stateMap[PlayerController.State.Idle]);
        }

        // Change JumpState
        if (player.IsJump && player.isGrounded)
        {
            player.stateMachine.ChangeState(player.stateMap[PlayerController.State.Jump]);
        }

        // Change Crawl state

    }
}
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
        Vector3 moveVector = InputManager.Instance.CurrentMoveVector;

        if (moveVector != Vector3.zero || (player.PlayerForce.x != 0 || player.PlayerForce.z != 0))
        {
            player.stateMachine.ChangeState(player.stateMap[PlayerController.State.Move]);
        }
        else
        {
            player.stateMachine.ChangeState(player.stateMap[PlayerController.State.Idle]);
        }

        if (player.IsJump && player.isGrounded)
        {
            player.stateMachine.ChangeState(player.stateMap[PlayerController.State.Jump]);
        }

        if (player.anim.GetInteger("up") == 4 && player.IsCrawl)
        {
            player.stateMachine.ChangeState(player.stateMap[PlayerController.State.Crawl]);
        }
        else if (player.anim.GetInteger("up") == 3 && !player.IsCrawl)
        {
            player.anim.SetInteger("up", 4);
            player.stateMachine.ChangeState(player.stateMap[PlayerController.State.Idle]);
        }
    }
}
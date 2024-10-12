using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using static UnityEngine.PlayerLoop.PreUpdate;

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
    public const float defaultDelayTime = 1f;

    public string GetName()
    {
        return this.GetType().Name;
    }

    public void HandleChangeState(PlayerController player)
    {        
        if (AnimationStates.IsInFreeFallState(player.anim))
        {   
            player.stateMachine.ChangeState(player.stateMap[PlayerController.State.Idle]);            
        }

        if (!player.IsTransitionAllowed)
        {
            //Debug.Log("PlayerController Is Not Allowed Transition");
            return;
        }

        Vector3 moveVector = InputManager.Instance.CurrentMoveVector;
        if (moveVector != Vector3.zero || (player.PlayerForce.x != 0 || player.PlayerForce.z != 0) && !player.IsJump && !player.IsClimb && player.isGrounded)
        {
            player.stateMachine.ChangeState(player.stateMap[PlayerController.State.Move]);
            return;
        }
        else
        {
            player.stateMachine.ChangeState(player.stateMap[PlayerController.State.Idle]);
        }

        if (player.anim.GetInteger("up") ==  4 && player.IsCrawl)
        {
            player.stateMachine.ChangeState(player.stateMap[PlayerController.State.Crawl]);
            return;
        }
        if (player.anim.GetInteger("up") == 3 && player.IsCrawl)
        {
            player.IsTransitionAllowed = false;
            player.IsCrawl = false;
            player.anim.SetInteger("up", 4);
            player.StartCoroutine(WaitForAnimationToEnd(player, AnimationStates.CrawlToUp));
            player.stateMachine.ChangeState(player.stateMap[PlayerController.State.Idle]);
            return;
        }

        if (player.IsJump && player.isGrounded)
        {
            player.stateMachine.ChangeState(player.stateMap[PlayerController.State.Jump]);
            return;
        }
    }

    public IEnumerator Wait(PlayerController player)
    {
        yield return new WaitForSeconds(defaultDelayTime);
        player.IsTransitionAllowed = true;
    }

    public virtual IEnumerator WaitForAnimationToEnd(PlayerController player, string stateName, float delayTime = defaultDelayTime)
    {
        yield return null;
        while (player.anim.IsInTransition(0))
        {
            yield return null;
        }

        AnimatorStateInfo stateInfo = player.anim.GetCurrentAnimatorStateInfo(0);
        if (!stateInfo.IsName(stateName))
        {
            // stateName != 현재프레임이라면, 한  프레임 더 대기 후 갱신
            yield return null;
            stateInfo = player.anim.GetCurrentAnimatorStateInfo(0);
        }

        if (stateInfo.normalizedTime >= 1.0f)
        {
            // Debug.Log($"{player.stateMachine.CurrentState.GetName()} : 진행 중인 애니메이션 트리거가 없음");
        }
        else if (!stateInfo.IsName(stateName))
        {
            yield return new WaitForSeconds(delayTime);
        }

        while (stateInfo.IsName(stateName) && stateInfo.normalizedTime < 1.0f)
        {
            yield return null;
            stateInfo = player.anim.GetCurrentAnimatorStateInfo(0);
        }

        player.IsTransitionAllowed = true;
    }

    public virtual IEnumerator WaitForAnimationToEnd(PlayerController player, List<string> stateNames, float delayTime = defaultDelayTime)
    {
        yield return null;
        foreach (string stateName in stateNames)
        {
            while (player.anim.IsInTransition(0))
            {
                yield return null;
            }

            AnimatorStateInfo stateInfo = player.anim.GetCurrentAnimatorStateInfo(0);

            if (!stateInfo.IsName(stateName))
            {                
                continue;
            }

            while (stateInfo.IsName(stateName) && stateInfo.normalizedTime < 1.0f)
            {
                yield return null;
                stateInfo = player.anim.GetCurrentAnimatorStateInfo(0);
            }
        }

        player.IsTransitionAllowed = true;
    }
}
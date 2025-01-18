using System.Collections.Generic;
using UnityEngine;

public static class AnimationStates
{
    public static readonly List<string> WalkJumpSatates = new List<string> { Jump, FreeFall, RunjumpOut, Land };
    public static readonly List<string> RunJumpStates = new List<string> { RunJump, RunJumpAir, FreeFall, RunjumpOut, Land };

    public const string UpToCrawl = "up_crawl";
    public const string CrawlToUp = "crawl_up";
    public const string Jump = "jump";
    public const string RunJump = "runjumpin";
    public const string RunJumpAir = "runjumpair";
    public const string FreeFall = "freefall";
    public const string RunjumpOut = "IP_runjumpAOUT";
    public const string Land = "land";
    
    public const string Spike = "Spike";    
    public const string TurningGrinder = "TurningGrinder";
    public const string Swing = "Swing";

    public static bool IsInFreeFallState(Animator animator, int layerIndex = 0)
    {
        AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(layerIndex);
        bool isInFreeFall = stateInfo.IsName(FreeFall) || stateInfo.IsName(Land);
        //if (isInFreeFall)
        //{
        //    Debug.Log($"FreeFall or Land 중입니다.");
        //}
        return isInFreeFall;
    }

    public static bool IsInWalkJumpStates(Animator animator, int layerIndex = 0)
    {
        AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(layerIndex);
        string currentState = stateInfo.IsName(stateInfo.shortNameHash.ToString()) ? stateInfo.shortNameHash.ToString() : string.Empty;
        if(WalkJumpSatates.Contains(currentState))
        {
            Debug.Log($"WalkStates 중 {currentState}가 포함되어 있음.");
        }
        return WalkJumpSatates.Contains(currentState);
    }

    public static bool IsInRunJumpStates(Animator animator, int layerIndex = 0)
    {
        AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(layerIndex);
        string currentState = stateInfo.IsName(stateInfo.shortNameHash.ToString()) ? stateInfo.shortNameHash.ToString() : string.Empty;
        if (RunJumpStates.Contains(currentState))
        {
            Debug.LogWarning($"RunJumpStates 중 {currentState}가 포함되어 있음.");
        }
        return RunJumpStates.Contains(currentState);
    }
}
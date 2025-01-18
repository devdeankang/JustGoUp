using System.Collections;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;

public class DisappearStrategy : IObstacleStrategy
{
    private DisappearConfig config;
    private bool isActive = true;
    private bool isRunning = false; // 코루틴 실행 상태

    public DisappearStrategy(StrategyConfig config)
    {
        this.config = config as DisappearConfig;
    }

    public void Execute(ObstacleController obstacle, bool hasAnimator)
    {
        if (!isActive || isRunning) return;
        obstacle.StartCoroutine(DisappearRoutine(obstacle));
    }

    private IEnumerator DisappearRoutine(ObstacleController obstacle)
    {
        isRunning = true;

        SetVisibility(obstacle, false);
        yield return new WaitForSeconds(config.disappearTime);

        if (config.isPermanentlyInactive)
        {
            obstacle.gameObject.SetActive(false);
            isRunning = false;

            yield break;
        }

        SetVisibility(obstacle, true);

        if (config.loopDisappear)
        {
            yield return new WaitForSeconds(config.reappearTime);
            isRunning = false;
            obstacle.StartCoroutine(DisappearRoutine(obstacle));
        }
        else
        {
            isRunning = false;
        }
    }

    private void SetVisibility(ObstacleController obstacle, bool isVisible)
    {
        foreach (var renderer in obstacle.GetComponentsInChildren<Renderer>())
        {
            renderer.enabled = isVisible;
        }

        foreach (var collider in obstacle.GetComponentsInChildren<Collider>())
        {
            collider.enabled = isVisible;
        }
    }

    public void SetPermanentInactive(bool value)
    {
        config.isPermanentlyInactive = value;
    }
}
using System.Collections;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;

public class DisappearStrategy : IObstacleStrategy
{
    DisappearConfig config;
    float timer;
    bool isDisappeared;

    public DisappearStrategy(StrategyConfig config)
    {
        this.config = config as DisappearConfig;
        timer = 0f;
        isDisappeared = false;
    }

    public void Execute(ObstacleController obstacle)
    {
        if (config == null) return;

        timer += Time.deltaTime;

        if (!isDisappeared && timer >= config.disappearTime)
        {
            obstacle.gameObject.SetActive(false);
            isDisappeared = true;
            timer = 0f;
        }
        else if (isDisappeared && config.reappearTime > 0 && timer >= config.reappearTime)
        {
            obstacle.gameObject.SetActive(true);
            isDisappeared = false;

            if (!config.loopDisappear)
            {
                timer = 0f;
            }
        }
    }

    public void UpdateConfig(float newDisappearTime, float newReappearTime, bool newLoopDisappear)
    {
        if (config != null)
        {
            config.disappearTime = newDisappearTime;
            config.reappearTime = newReappearTime;
            config.loopDisappear = newLoopDisappear;
            timer = 0f;
        }
    }
}
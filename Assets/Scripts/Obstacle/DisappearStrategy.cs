using System.Collections;
using UnityEngine;

public class DisappearStrategy : IObstacleStrategy
{
    float disappearTime;
    float timer;

    public DisappearStrategy(float disappearTime)
    {
        this.disappearTime = disappearTime;
        this.timer = 0f;
    }

    public void Execute(ObstacleController obstacle)
    {
        timer += Time.deltaTime;
        if(timer >= disappearTime)
        {
            //@@ Consider using a PoolManager to handle object pooling. 
            obstacle.gameObject.SetActive(false);
        }
    }
    
}
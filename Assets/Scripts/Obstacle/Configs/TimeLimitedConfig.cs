using UnityEngine;

[CreateAssetMenu(fileName = "TimeLimitedConfig", menuName = "Obstacle/TimeLimitedConfig")]
public class TimeLimitedConfig : StrategyConfig
{
    public float timeLimit;
    public StrategyConfig innerStrategyConfig; // 내부 장애물 전략을 위한
}
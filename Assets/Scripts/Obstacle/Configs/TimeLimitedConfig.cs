using UnityEngine;

[CreateAssetMenu(fileName = "TimeLimitedConfig", menuName = "Obstacle/TimeLimitedConfig")]
public class TimeLimitedConfig : StrategyConfig
{
    public float timeLimit;
    public StrategyConfig innerStrategyConfig; // ���� ��ֹ� ������ ����
}
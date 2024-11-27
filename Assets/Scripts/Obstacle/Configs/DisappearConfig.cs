using UnityEngine;

[CreateAssetMenu(fileName = "DisappearConfig", menuName = "Obstacle/DisappearConfig")]
public class DisappearConfig : StrategyConfig
{
    public float disappearTime;
    public float reappearTime;
    public bool loopDisappear;
}

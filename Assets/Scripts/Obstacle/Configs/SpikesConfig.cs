using UnityEngine;

[CreateAssetMenu(fileName = "SpikesConfig", menuName = "Obstacle/SpikesConfig")]
public class SpikesConfig : StrategyConfig
{
    public float spikeInterval;
    public float spikeHeight;
    public float riseSpeed;
    public float fallSpeed;
    public float knockbackAngle;
    public float knockbackForce;
}
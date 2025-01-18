using UnityEngine;

[CreateAssetMenu(fileName = "TurningGrinderConfig", menuName = "Obstacle/TurningGrinderConfig")]
public class TurningGrinderConfig : StrategyConfig
{
    public float turningSpeed;
    public Vector3 rotationAxis = Vector3.up;
    public float rotationRadius;
    public float knockbackForce;
}

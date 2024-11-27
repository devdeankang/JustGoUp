using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PushConfig", menuName = "Obstacle/PushConfig")]
public class PushConfig : StrategyConfig
{
    public float pushSpeed;
    public float pushForce;
    public Vector3 pushDirection;
}

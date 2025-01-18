using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PushConfig", menuName = "Obstacle/PushConfig")]
public class PushConfig : StrategyConfig
{
    public GameObject pushPrefab;
    public float pushSpeed;
    public float pushForce;
    public Vector3 pushDirection;
    public float lifeTime = 3f;
    public float spawnInterval = 2f;
}

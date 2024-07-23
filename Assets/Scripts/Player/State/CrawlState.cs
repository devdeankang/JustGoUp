using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrawlState : State<PlayerController>
{
    public override void Enter(PlayerController player)
    {
        
    }

    public override void Update(PlayerController player)
    {
        HandleChangeState(player);
    }

    public override void FixedUpdate(PlayerController player)
    {
        
    }

    public override void Exit(PlayerController player)
    {
        
    }
}
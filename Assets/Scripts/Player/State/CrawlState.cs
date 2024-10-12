using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static UnityEngine.PlayerLoop.PreUpdate;

public class CrawlState : State<PlayerController>
{
    PlayerController player;
    bool canUpdate = false;
    
    public override void Enter(PlayerController player)
    {
        this.player = player;
                
        canUpdate = false;
        player.IsCrawl = false;
        player.IsTransitionAllowed = false;
        player.anim.SetInteger("up", 3);
        player.rayCorrection = 0.2f;
        
        SetCrawlState();
        
        player.StartCoroutine(WaitForAnimationToEnd(player, AnimationStates.UpToCrawl));
    }

    public override void Update(PlayerController player)
    {        
        if (canUpdate)
        {
            HandleChangeState(player);
        }
    }

    public override void FixedUpdate(PlayerController player)
    {
        
    }

    public override void Exit(PlayerController player)
    {
        
    }

    private void SetCrawlState()
    {
        if (player.anim.GetInteger("up") == 3 && player.PlayerForce != Vector3.zero)
        {
            player.tr.rotation = Quaternion.LookRotation(player.PlayerForce, Vector3.up);
        }

        if (!player.isGrounded && Input.GetAxis("Vertical") > 0.01f)
        {
            player.rb.AddForce(player.tr.forward * 0.05f, ForceMode.Impulse);
        }

        if (!player.isGrounded && Input.GetAxis("Vertical") < -0.01f)
        {
            player.rb.AddForce(player.tr.forward * -0.05f, ForceMode.Impulse);
        }

        Vector3 forcedir3 = new Vector3(player.PlayerForce.x, 0f, player.PlayerForce.z);
        if (Physics.Raycast(player.tr.position + (player.tr.forward * player.rayCorrection) + new Vector3(0f, 0.1f, 0f), Vector3.down, out player.hit))
        {
            forcedir3 = -Vector3.Cross(Vector3.Cross(player.hit.normal, player.tr.forward), player.hit.normal);
            forcedir3.Normalize();
        }
        Vector3 forcedir2 = new Vector3(player.PlayerForce.x, 0f, player.PlayerForce.z);
        if (Physics.Raycast(player.tr.position + (player.tr.forward * -0.05f) + new Vector3(0f, 0.1f, 0f), Vector3.down, out player.hit2))
        {
            forcedir2 = -Vector3.Cross(Vector3.Cross(player.hit2.normal, player.tr.forward), player.hit2.normal);
            forcedir2.Normalize();
        }
        player.PlayerForce = (forcedir3 + forcedir2);
        player.PlayerForce.Normalize();

        if (player.isGrounded)
        {
            player.rb.velocity = player.PlayerForce * player.anim.GetFloat("speed");
        }        
    }

    public override IEnumerator WaitForAnimationToEnd(PlayerController player, string stateName, float delayTime = 1)
    {
        yield return base.WaitForAnimationToEnd(player, stateName, delayTime);
        canUpdate = true;
    }
}
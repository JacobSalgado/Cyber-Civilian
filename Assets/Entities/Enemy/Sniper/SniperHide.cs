using System.Collections.Generic;
using UnityEditor.ShaderGraph.Internal;
using UnityEngine;

public class SniperHide : State
{
    readonly Sniper sniper;

    private float hideTimer; // will count how long for sniper to stay invisible
    private float hideTime = Random.Range(3.0f, 6.0f);



    public SniperHide(Entity new_entity) : base(new_entity)
    {
        sniper = (Sniper)new_entity;
    }

    public override void EnterState(Dictionary<string, object> args = null)
    {
        sniper.timer = 0;
        sniper.Invisible();

        sniper.isRecharging = true; // resets
    }

    public override void UpdateState()
    {
        hideTimer += Time.deltaTime;

        if (sniper.target == null)
        {
            Debug.LogError("Target not found");
            return;
        }

        // TODO: implement hiding behavior
        /*if (sniper.GetDistanceToTarget() < 1f)
        {
            sniper.Invisible();
        }*/

        if (hideTimer > hideTime)
        {
            hideTimer = 0;
            sniper.ChangeState("Idle");
        }
    }
    
    public override void ExitState(Dictionary<string, object> args = null)
    {
        
    }
}
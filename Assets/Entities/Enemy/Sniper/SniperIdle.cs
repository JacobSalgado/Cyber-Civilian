using System.Collections.Generic;
using UnityEngine;

public class SniperIdle : State
{
    readonly Sniper sniper;

    public SniperIdle(Entity new_entity) : base(new_entity)
    {
        sniper = (Sniper)new_entity;
    }

    public override void EnterState(Dictionary<string, object> args = null)
    {
        sniper.Visible();
    }

    public override void UpdateState()
    {
        sniper.timer += Time.deltaTime; // hide recharge

        if (sniper.timer > sniper.rechargeTime)
        {
            sniper.isRecharging = false;
        }

        if (sniper.target == null)
        {
            Debug.LogError("Target not found");
            return;
        }

        if (sniper.GetDistanceToTarget() < 10f && !sniper.isRecharging && sniper.timer > sniper.rechargeTime)
        {
            sniper.ChangeState("Hide");
        }

        float distanceToTarget = sniper.GetDistanceToTarget();
        Debug.Log(sniper.GetDistanceToTarget());
        if (distanceToTarget < sniper.distanceToShoot)
            sniper.ChangeState("Shoot");
        //else if (distanceToTarget < sniper.distanceToHide)
        //    sniper.ChangeState("Hide");        
    }
    
    public override void ExitState(Dictionary<string, object> args = null)
    {
        
    }
}
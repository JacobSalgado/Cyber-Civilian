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

    }

    public override void UpdateState()
    {
        if (sniper.target == null)
        {
            Debug.LogError("Target not found");
            return;
        }

        float distanceToTarget = sniper.GetDistanceToTarget();

        if (distanceToTarget < sniper.distanceToHide && !sniper.isInvisibleRecharging && sniper.timer > sniper.invisibleRechargeTime)
        {
            sniper.ChangeState("Hide");
            return;
        }
        if (distanceToTarget < sniper.distanceToShoot)
            sniper.ChangeState("Shoot");
    }
    
    public override void ExitState(Dictionary<string, object> args = null)
    {
        
    }
}
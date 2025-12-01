using System.Collections.Generic;
using UnityEngine;

public class CrabIdle : State
{
    readonly Crab crab;

    public CrabIdle(Entity new_entity) : base(new_entity)
    {
        crab = (Crab)new_entity;
    }

    public override void EnterState(Dictionary<string, object> args = null)
    {
        crab.moveVelocity = Vector2.zero;
        crab.PlayAnim("Idle");
    }

    public override void UpdateState()
    {
        if (crab.target == null)
        {
            Debug.LogError("Target not found");
            return;
        }

        float distanceToTarget = crab.GetDistanceToTarget();

        crab.moveVelocity = Vector2.zero;

        if (distanceToTarget < crab.distanceToHide && !crab.isInvisibleRecharging && crab.timer > crab.invisibleRechargeTime)
        {
            crab.ChangeState("Hide");
            return;
        }
        if (distanceToTarget < crab.distanceToShoot)
            crab.ChangeState("Shoot");
    }
    
    public override void ExitState(Dictionary<string, object> args = null)
    {
        
    }
}
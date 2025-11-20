using System.Collections.Generic;
using UnityEngine;

public class HomingIdle : State
{
    readonly Homing homing;

    public HomingIdle(Entity new_entity) : base(new_entity)
    {
        homing = (Homing)new_entity;
    }

    public override void EnterState(Dictionary<string, object> args = null)
    {
        homing.moveVelocity = Vector2.zero;
        homing.PlayAnim("Idle");
    }

    public override void UpdateState()
    {
        if (homing.target == null)
        {
            Debug.LogError("Target not found");
            return;
        }

        homing.moveVelocity = Vector2.zero;

        float distanceToTarget = homing.GetDistanceToTarget();
        if (distanceToTarget < homing.distanceToShoot)
            homing.ChangeState("Shoot");
        else if (distanceToTarget < homing.distanceToMove)
            homing.ChangeState("Move");
    }
}

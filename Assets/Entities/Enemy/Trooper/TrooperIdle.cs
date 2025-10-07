using System.Collections.Generic;
using UnityEngine;

public class TrooperIdle : State
{
    readonly Trooper trooper;

    public TrooperIdle(Entity new_entity) : base(new_entity)
    {
        trooper = (Trooper) new_entity;
    }

    public override void EnterState(Dictionary<string, object> args = null)
    {
        trooper.rigidBody.linearVelocity = Vector2.zero;
    }

    public override void UpdateState()
    {
        float distance = trooper.GetDistanceToTarget();
        if (distance > -1f)
        {
            if (distance < trooper.distanceToShoot)
                trooper.ChangeState("Shoot");
            else if (distance < trooper.distanceToMove)
                trooper.ChangeState("Move");
        }
    }

    public override void ExitState(Dictionary<string, object> args = null)
    {
        
    }
}

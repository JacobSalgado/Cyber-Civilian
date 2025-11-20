using System.Collections.Generic;
using UnityEngine;

public class TrapperIdle : State
{
    readonly Trapper trapper;

    public TrapperIdle(Entity new_entity) : base(new_entity)
    {
        trapper = (Trapper) new_entity;
    }

    public override void EnterState(Dictionary<string, object> args = null)
    {
        trapper.moveVelocity = Vector2.zero;
        trapper.PlayAnim("Idle");
    }

    public override void UpdateState()
    {
        if (trapper.target == null)
        {
            Debug.LogError("Target not found");
            return;
        }

        float distanceToTarget = trapper.GetDistanceToTarget();
        if (distanceToTarget <= -1f)
        { // assertion check
            Debug.Log("target not set");
            return;
        }
        
        trapper.moveVelocity = Vector2.zero;
         
        if (distanceToTarget < trapper.distanceToShoot)
            trapper.ChangeState("Shoot");
        else if (distanceToTarget < trapper.distanceToMove)
            trapper.ChangeState("Move");
        
    }

    public override void ExitState(Dictionary<string, object> args = null)
    {
        
    }
}

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
        if (trooper.target == null)
        {
            Debug.LogError("Target not found");
            return;
        }

        float distanceToTarget = trooper.GetDistanceToTarget();
        if (distanceToTarget <= -1f)
        { // assertion check
            Debug.Log("target not set");
            return;
        }
        
        trooper.rigidBody.linearVelocity = Vector2.zero;
         
        if (distanceToTarget < trooper.distanceToShoot)
            trooper.ChangeState("Shoot");
        else if (distanceToTarget < trooper.distanceToMove)
            trooper.ChangeState("Move");
        
    }

    public override void ExitState(Dictionary<string, object> args = null)
    {
        
    }
}

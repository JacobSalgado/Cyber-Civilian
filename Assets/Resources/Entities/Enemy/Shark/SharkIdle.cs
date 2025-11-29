using System.Collections.Generic;
using UnityEngine;

public class SharkIdle : State
{
    readonly Shark shark;

    public SharkIdle(Entity new_entity) : base(new_entity)
    {
        shark = (Shark) new_entity;
    }

    public override void EnterState(Dictionary<string, object> args = null)
    {
        shark.moveVelocity = Vector2.zero;
        shark.PlayAnim("Idle");
    }

    public override void UpdateState()
    {
        if (shark.target == null)
        {
            Debug.LogError("Target not found");
            return;
        }

        float distanceToTarget = shark.GetDistanceToTarget();
        if (distanceToTarget <= -1f)
        { // assertion check
            Debug.Log("target not set");
            return;
        }

        shark.moveVelocity = Vector2.zero;
         
        if (distanceToTarget < shark.distanceToShoot)
            shark.ChangeState("Shoot");
        else if (distanceToTarget < shark.distanceToMove)
            shark.ChangeState("Move");
        
    }

    public override void ExitState(Dictionary<string, object> args = null)
    {
        
    }
}

using System.Collections.Generic;
using UnityEngine;

public class FighterMove : State
{
    readonly Fighter fighter;
    private Vector2 directionToTarget;

    public FighterMove(Entity new_entity) : base(new_entity)
    {
        fighter = (Fighter)new_entity;
    }

    public override void EnterState(Dictionary<string, object> args = null)
    {

    }

    public override void UpdateState()
    {
        if (fighter.target == null)
        {
            Debug.LogError("Target not found");
            return;
        }

        float distance = fighter.GetDistanceToTarget();
        if (distance > -1f)
        {
            if (distance < fighter.distanceToHit)
            {
                fighter.ChangeState("Hit");
                return;
            }
            else if (distance < fighter.distanceToMove)
            {
                directionToTarget = fighter.GetDirectionToPosition(fighter.target.transform.position);
                fighter.RotateToDirection(directionToTarget);
                fighter.rigidBody.linearVelocity = directionToTarget * fighter.entityData.moveSpeed;
            }
            else fighter.ChangeState("Idle");
        }
    }

    public override void ExitState(Dictionary<string, object> args = null)
    {
        
    }
}

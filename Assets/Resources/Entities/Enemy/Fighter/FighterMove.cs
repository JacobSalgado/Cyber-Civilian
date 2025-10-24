using System.Collections.Generic;
using UnityEngine;

public class FighterMove : State
{
    private const float timeToStep = 0.3f;

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

        fighter.deltaCount += Time.deltaTime;

        if (fighter.PlayFootsteps(fighter.deltaCount, timeToStep, fighter.stepCounter))
        {
            fighter.deltaCount = 0f;
            fighter.stepCounter++;
            if (fighter.stepCounter > 3) fighter.stepCounter = 1;
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

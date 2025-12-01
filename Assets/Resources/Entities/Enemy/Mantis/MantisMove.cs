using System.Collections.Generic;
using UnityEngine;

public class MantisMove : State
{
    private const float timeToStep = 0.3f;

    readonly Mantis mantis;
    private Vector2 directionToTarget;

    public MantisMove(Entity new_entity) : base(new_entity)
    {
        mantis = (Mantis)new_entity;
    }

    public override void EnterState(Dictionary<string, object> args = null)
    {
        mantis.PlayAnim("Walk");
    }

    public override void UpdateState()
    {
        if (mantis.target == null)
        {
            Debug.LogError("Target not found");
            return;
        }

        mantis.deltaCount += Time.deltaTime;

        if (mantis.PlayFootsteps(mantis.deltaCount, timeToStep, mantis.stepCounter))
        {
            mantis.deltaCount = 0f;
            mantis.stepCounter++;
            if (mantis.stepCounter > 3) mantis.stepCounter = 1;
        }

        float distance = mantis.GetDistanceToTarget();
        if (distance > -1f)
        {
            if (distance < mantis.DISTANCE_TO_PUNCH)
            {
                mantis.ChangeState("Punch");
                return;
            }
            else if (distance < mantis.distanceToMove)
            {
                directionToTarget = mantis.GetDirectionToPosition(mantis.target.transform.position);
                mantis.RotateToDirection(directionToTarget);
                mantis.moveVelocity = directionToTarget * mantis.entityData.moveSpeed;
            }
            else mantis.ChangeState("Idle");
        }
    }

    public override void ExitState(Dictionary<string, object> args = null)
    {
        
    }
}

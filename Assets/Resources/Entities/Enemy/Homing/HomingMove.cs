using UnityEngine;
using System;
using System.Collections.Generic;

public class HomingMove : State
{
    readonly Homing homing;

    private const float timeToStep = 0.3f;

    private Vector2 directionToTarget;


    public HomingMove(Entity new_entity) : base(new_entity)
    {
        homing = (Homing)new_entity;
    }

    public override void EnterState(System.Collections.Generic.Dictionary<string, object> args = null)
    {
        homing.PlayAnim("Walk");
    }

    public override void UpdateState()
    {
        if (homing.target == null)
        {
            Debug.LogError("Target not found");
            return;
        }

        homing.deltaCount += Time.deltaTime;

        if (homing.PlayFootsteps(homing.deltaCount, timeToStep, homing.stepCounter))
        {
            homing.deltaCount = 0f;
            homing.stepCounter++;
            if (homing.stepCounter > 3) homing.stepCounter = 1;
        }

        float distance = homing.GetDistanceToTarget();
        if (distance > -1f)
        {
            if (distance < homing.distanceToShoot)
            {
                homing.ChangeState("Shoot");
                return;
            }
            else if (distance < homing.distanceToMove)
            {
                directionToTarget = homing.GetDirectionToPosition(homing.target.transform.position);
                homing.RotateToDirection(directionToTarget);
                homing.moveVelocity = directionToTarget * homing.entityData.moveSpeed;
            }
            else homing.ChangeState("Idle");
        }
    }

    public override void ExitState(Dictionary<string, object> args = null)
    {
        homing.moveVelocity = Vector2.zero;
    }

}

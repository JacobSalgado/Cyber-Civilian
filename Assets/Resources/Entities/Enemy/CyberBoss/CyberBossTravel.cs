using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class CyberBossTravel: State
{
    readonly CyberBoss cyberBoss;

    //private float checkInterval = 0.5f;
    //private float nextCheckTime;

    private const float timeToStep = 0.3f;

    private Vector2 directionToTarget;
    private float deltaCount = 0f;
    int stepCounter = 1;

    public CyberBossTravel(Entity new_entity) : base(new_entity)
    {
        cyberBoss = (CyberBoss)new_entity;
    }

    public override void EnterState(Dictionary<string, object> args = null)
    {
        deltaCount = 0f;
        stepCounter = 1;
    }

    public override void UpdateState()
    {
        if (cyberBoss.target == null)
        {
            Debug.LogError("target not found");
            return;
        }

        deltaCount += Time.deltaTime;

        if (cyberBoss.PlayFootsteps(deltaCount, timeToStep, stepCounter))
        {
            deltaCount = 0f;
            stepCounter++;
            if (stepCounter > 3) stepCounter = 1;
        }

        //if (!cyberBoss.HasSeenPlayerLongEnough)
        //{
        //    cyberBoss.ChangeState("Idle");
        //    return;
        //}

        float distance = cyberBoss.GetDistanceToTarget();
        
            if (distance <= cyberBoss.distanceToHit)
            {
                cyberBoss.ChangeState("Punch");
                return;
            }
            else if (distance < cyberBoss.distanceToShoot)
            {
                cyberBoss.ChangeState("Missile");
                return;
            }
            else if (distance < cyberBoss.distanceToStomp)
            {
                cyberBoss.ChangeState("Stomp");
                return;
            }
            else if (distance < cyberBoss.distanceToMove)
            {
                directionToTarget = cyberBoss.GetDirectionToPosition(cyberBoss.target.transform.position);
                cyberBoss.RotateToDirection(directionToTarget);
                cyberBoss.rigidBody.linearVelocity = directionToTarget * cyberBoss.entityData.moveSpeed;
            }
            else cyberBoss.ChangeState("Idle");
        
    }

    public override void ExitState(Dictionary<string, object> args = null)
    {
        base.ExitState(args);
    }
}

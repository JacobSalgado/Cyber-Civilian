using UnityEngine;
using System.Collections.Generic;

public class CyberBossTravel: State
{
    readonly CyberBoss cyberBoss;

    //private float checkInterval = 0.5f;
    //private float nextCheckTime;

    private const float timeToStep = 0.5f;

    private Vector2 directionToTarget;
    private float deltaCount = 0f;
    int stepCounter = 1;

    public CyberBossTravel(Entity new_entity) : base(new_entity)
    {
        cyberBoss = (CyberBoss)new_entity;
    }

    public override void EnterState(Dictionary<string, object> args = null)
    {
        Debug.Log("In travel state");
        deltaCount = 0f;
        stepCounter = 1;
    }

    public override void UpdateState()
    {
        Debug.Log("cyberboss travel state");
        
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

        if (distance <= cyberBoss.distanceToHit && !cyberBoss.inCooldown)
        {
            cyberBoss.ChangeState("Punch");
            return;
        }
        else if (distance < cyberBoss.distanceToStomp && !cyberBoss.inCooldown)
        {
            cyberBoss.ChangeState("Stomp");
            return;
        }
        else if (distance < cyberBoss.distanceToShoot && !cyberBoss.inCooldown)
        {
            cyberBoss.ChangeState("Missile");
            return;
        }
        else if (distance < cyberBoss.distanceToMove)
        {
            directionToTarget = cyberBoss.GetDirectionToPosition(cyberBoss.target.transform.position);
            cyberBoss.RotateToDirection(directionToTarget);
            cyberBoss.moveVelocity = directionToTarget * cyberBoss.entityData.moveSpeed;
        }
        else cyberBoss.ChangeState("Idle");
        
    }

    public override void ExitState(Dictionary<string, object> args = null)
    {
        cyberBoss.moveVelocity = Vector2.zero;
    }
}

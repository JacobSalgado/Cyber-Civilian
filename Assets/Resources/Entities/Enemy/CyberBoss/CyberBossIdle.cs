using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class CyberBossIdle: State
{
    readonly CyberBoss cyberBoss;

    public CyberBossIdle(Entity new_entity) : base(new_entity)
    {
        cyberBoss = (CyberBoss) new_entity;
    }

    public override void EnterState(Dictionary<string, object> args = null)
    {
        Debug.Log("In Idle state");
        cyberBoss.moveVelocity = Vector2.zero;
        //cyberBoss.PlayAnim("Idle");
    }

    public override void UpdateState()
    {
        cyberBoss.UpdateVision(); // raycast vision to lock on player

        if (cyberBoss.target == null)
        {
            Debug.LogError("Target not found");
            return;
        }

        cyberBoss.moveVelocity = Vector2.zero; // cyberboss stays still

        if (cyberBoss.HasSeenPlayerLongEnough)
        {
            cyberBoss.ChangeState("Travel");
            return;
        }

        float distanceToTarget = cyberBoss.GetDistanceToTarget();

        if (distanceToTarget <= cyberBoss.distanceToHit && !cyberBoss.inCooldown)
            cyberBoss.ChangeState("Punch");
        else if (distanceToTarget < cyberBoss.distanceToStomp && !cyberBoss.inCooldown)
            cyberBoss.ChangeState("Stomp");
        else if (distanceToTarget < cyberBoss.distanceToShoot && !cyberBoss.inCooldown)
            cyberBoss.ChangeState("Missile");
        else if (distanceToTarget < cyberBoss.distanceToMove && !cyberBoss.inCooldown)
            cyberBoss.ChangeState("Travel");
    }

    public override void ExitState(Dictionary<string, object> args = null)
    {
        
    }
}

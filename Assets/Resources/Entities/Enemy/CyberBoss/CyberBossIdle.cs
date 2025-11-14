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
        cyberBoss.rigidBody.linearVelocity = Vector2.zero;
        cyberBoss.PlayAnim("Idle");
    }

    public override void UpdateState()
    {
        if (cyberBoss.target == null)
        {
            Debug.LogError("Target not found");
            return;
        }

        cyberBoss.rigidBody.linearVelocity = Vector2.zero; // cyberboss stays still

        float distanceToTarget = cyberBoss.GetDistanceToTarget();

        if (distanceToTarget < cyberBoss.distanceToHit)
            cyberBoss.ChangeState("Punch");
        else if (distanceToTarget < cyberBoss.distanceToShoot)
            cyberBoss.ChangeState("Missile");
        else if (distanceToTarget < cyberBoss.distanceToStomp)
            cyberBoss.ChangeState("Stomp");
        else if (distanceToTarget < cyberBoss.distanceToMove)
            cyberBoss.ChangeState("Travel");
    }

    public override void ExitState(Dictionary<string, object> args = null)
    {
        base.ExitState(args);
    }
}

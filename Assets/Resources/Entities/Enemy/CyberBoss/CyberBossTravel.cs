using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class CyberBossTravel: State
{
    readonly CyberBoss cyberBoss;

    private float checkInterval = 0.5f;
    private float nextCheckTime;

    public CyberBossTravel(Entity new_entity) : base(new_entity)
    {
        cyberBoss = (CyberBoss)new_entity;
    }

    public override void EnterState(Dictionary<string, object> args = null)
    {
        base.EnterState(args);
        nextCheckTime = Time.time + checkInterval;
    }

    public override void UpdateState()
    {
        if (cyberBoss.Target == null)
        {
            cyberBoss.ChangeState("Idle");
            return;
        }

        float distanceToTarget = cyberBoss.GetDistanceToTarget();

        // move to target player
        cyberBoss.MoveTowardsTarget();

        // checks if we should switch states
        if (Time.time >= nextCheckTime)
        {
            nextCheckTime = Time.time + checkInterval;

            if (distanceToTarget <= cyberBoss.StompRange && cyberBoss.CanStompAttack())
            {
                cyberBoss.ChangeState("StompAttack");
                return;
            }

            if (distanceToTarget <= cyberBoss.AttackRange && cyberBoss.CanMissileAttack())
            {
                // 60% chance to launch missile attack, 40% chance to keep traveling closer
                if (Random.value > 0.4f)
                {
                    cyberBoss.ChangeState("MissileAttack");
                    return;
                }
            }

            if (distanceToTarget <= cyberBoss.StopDistance)
            {
                cyberBoss.ChangeState("Idle");
                return;
            }
        }
    }

    public override void ExitState(Dictionary<string, object> args = null)
    {
        cyberBoss.StopMovement();
        base.ExitState(args);
    }
}

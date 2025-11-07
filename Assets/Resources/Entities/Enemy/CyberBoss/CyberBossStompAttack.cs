using UnityEngine;
using System.Collections.Generic;

public class CyberBossStompAttack: State
{
    readonly CyberBoss cyberBoss;

    private bool hasStomped = false;
    private float stompDelay = 0.3f; // wind-up time before stomp
    private float stompTime;

    public CyberBossStompAttack(Entity new_entity) : base(new_entity)
    {
        cyberBoss = (CyberBoss) new_entity;
    }

    public override void EnterState(Dictionary<string, object> args = null)
    {
        base.EnterState(args);
        
        // Stop movement
        cyberBoss.rigidBody.linearVelocity = Vector2.zero;

        // reset stomp flag
        hasStomped = false;
        stompTime = Time.time + stompDelay;
    }

    public override void UpdateState()
    {
        // stay still for attack
        cyberBoss.rigidBody.linearVelocity = Vector2.zero;

        // Perform stomp after delay
        if (!hasStomped && Time.time >= stompTime)
        {
            hasStomped = true;
            cyberBoss.StartStompAttack();

            // transition to idle happens in StompVisualFeedback in PerformStomp
        }
    }

    public override void ExitState(Dictionary<string, object> args = null)
    {
        base.ExitState(args);
    }

}

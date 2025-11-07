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
    }

    public override void UpdateState()
    {
        cyberBoss.rigidBody.linearVelocity = Vector2.zero;
    }

    public override void ExitState(Dictionary<string, object> args = null)
    {
        base.ExitState(args);
    }
}

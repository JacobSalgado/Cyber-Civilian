using UnityEngine;
using System.Collections.Generic;

public class CyberBossMissileAttack: State
{
    readonly CyberBoss cyberBoss;

    public CyberBossMissileAttack(Entity new_entity) : base(new_entity)
    {
        cyberBoss = (CyberBoss) new_entity;
    }

    public override void EnterState(Dictionary<string, object> args = null)
    {
        base.EnterState(args);

        cyberBoss.rigidBody.linearVelocity = Vector2.zero;

        cyberBoss.StartMissileVolley();
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

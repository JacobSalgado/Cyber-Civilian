using System.Collections.Generic;
using UnityEngine;

public class EnemyIdle : State
{
    readonly Enemy enemy;

    public EnemyIdle(Entity new_entity) : base(new_entity)
    {
        enemy = (Enemy)new_entity;
    }

    public override void EnterState(Dictionary<string, object> args = null)
    {
        enemy.rigidBody.linearVelocity = Vector2.zero;
    }

    public override void UpdateState()
    {
        
    }

    public override void ExitState(Dictionary<string, object> args = null)
    {
        
    }
}

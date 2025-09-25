using System.Collections.Generic;
using UnityEngine;

public class EnemyMove : State
{
    readonly Enemy enemy;

    public EnemyMove(Entity new_entity) : base(new_entity)
    {
        enemy = (Enemy) new_entity;
    }

    public override void EnterState(Dictionary<string, object> args = null) { }

    public override void UpdateState()
    {
        if (enemy.target == null)
        {
            enemy.ChangeState("Idle");
            return;
        }

        Vector2 direction = (enemy.target.position - enemy.transform.position).normalized;
        enemy.rb.linearVelocity = direction * enemy.entityData.moveSpeed;
    }

    public override void ExitState(Dictionary<string, object> args = null)
    {
        enemy.rb.linearVelocity = Vector2.zero;
    }
}

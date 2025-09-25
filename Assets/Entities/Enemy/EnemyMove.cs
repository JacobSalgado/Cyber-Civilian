using System.Collections.Generic;
using UnityEngine;

public class EnemyMove : State
{
    Enemy enemy;

    public EnemyMove(Entity new_entity) : base(new_entity)
    {
        enemy = (Enemy)new_entity;
    }

    public override void EnterState(Dictionary<string, object> args = null) { }

    public override void UpdateState()
    {
        if (enemy.target == null)
        {
            entity.ChangeState("Idle");
            return;
        }

        Vector2 direction = (enemy.target.position - entity.transform.position).normalized;
        entity.rb.linearVelocity = direction * entity.entityData.moveSpeed;
    }

    public override void ExitState(Dictionary<string, object> args = null)
    {
        entity.rb.linearVelocity = Vector2.zero;
    }
}

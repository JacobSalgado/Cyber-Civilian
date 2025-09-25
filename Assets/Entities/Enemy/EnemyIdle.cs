using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class EnemyIdle : State
{
    Enemy enemy;

    public EnemyIdle(Entity new_entity) : base(new_entity)
    {
        enemy = (Enemy)new_entity;
    }

    public override void EnterState(Dictionary<string, object> args = null)
    {
        entity.rb.linearVelocity = Vector2.zero;
    }

    public override void UpdateState()
    {
        if (enemy.target != null)
        {
            float distance = Vector2.Distance(entity.transform.position, enemy.target.position);
            if (distance < 5f)
            {
                entity.ChangeState("Move");
            }
        }
    }

    public override void ExitState(Dictionary<string, object> args = null)
    {
        
    }
}

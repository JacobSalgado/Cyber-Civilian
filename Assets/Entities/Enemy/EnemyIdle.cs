using System.Collections.Generic;
using UnityEngine;

public class EnemyIdle : State
{
    readonly Enemy enemy;
    private float fireTimer = 0f;
    private float fireRate = 5f;

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
        if (enemy.target != null)
        {
            float distance = Vector2.Distance(enemy.transform.position, enemy.target.position);
            if (distance < 10f)
            {
                Vector2 direction = (enemy.target.position - enemy.transform.position).normalized;
                enemy.RotateToDirection(direction);

                fireTimer -= Time.deltaTime;
                if (fireTimer <= 0f)
                {
                    enemy.ShootProjectile(enemy.proj, enemy.firePoint, 7);
                    fireTimer += 1f / fireRate;
                }
            }
            else
            {
                fireTimer = 0f;
            }
        }
    }

    public override void ExitState(Dictionary<string, object> args = null)
    {
        
    }
}

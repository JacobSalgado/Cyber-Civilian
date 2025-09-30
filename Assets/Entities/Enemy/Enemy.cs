using System;
using UnityEngine;

public class Enemy : Entity
{
    public Transform target; // following the player
    public GameObject proj;

    public override void InitializeStates()
    {
        AddState("Idle", new EnemyIdle(this));
        AddState("Move", new EnemyMove(this));

        ChangeState("Idle");
    }

    public override void Start()
    {
        base.Start();
        InitializeStates();
    }

    public override void FixedUpdate()
    {
        base.FixedUpdate();

        if (CanEnemyShoot())
        {
            Vector2 direction = (target.position - transform.position).normalized;
            RotateToDirection(direction);

            fireTimer -= Time.deltaTime;
            if (fireTimer <= 0f)
            {
                ShootProjectile(proj, firePoint, 7);
                fireTimer += 1f / fireRate;
            }
        }
    }

    private bool CanEnemyShoot()
    {
        if (target != null)
        {
            float distance = Vector2.Distance(transform.position, target.position);
            return distance < 10f;
        }
        else return false;
    }

    public override void OnCollisionEnter2D(Collision2D collision)
    {

    }
}

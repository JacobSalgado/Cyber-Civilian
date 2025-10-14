using System;
using UnityEngine;

public class Enemy : Entity
{
    public Transform target; // following the player
    public GameObject proj;

    // TODO: add weapon variable

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

            // TODO: enemy shoot weapon here
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

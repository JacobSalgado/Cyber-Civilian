using UnityEngine;

public class Enemy : Entity
{
    public Transform target; // following the player
    public GameObject proj;
    public Transform firePoint;

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

    public override void OnCollisionEnter2D(Collision2D collision)
    {

    }
}

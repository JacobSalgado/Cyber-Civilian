using UnityEngine;

public class Shark : Enemy
{
    [Header("==Shark Properties==")]
    public float distanceToShoot;
    public float distanceToMove;

    public override void InitializeStates()
    {
        AddState("Idle", new SharkIdle(this));
        AddState("Move", new SharkMove(this));
        AddState("Shoot", new SharkShoot(this));

        ChangeState("Idle");
    }

    public override void Start()
    {
        base.Start();
        InitializeStates();
        type = EnemyTypes.SHARK;
    }
}

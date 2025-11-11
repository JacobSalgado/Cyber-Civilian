using UnityEngine;

public class Trapper : Enemy
{
    [Header("==Trapper Properties==")]
    public float distanceToShoot;
    public float distanceToMove;

    public override void InitializeStates()
    {
        AddState("Idle", new TrapperIdle(this));
        AddState("Move", new TrapperMove(this));
        AddState("Shoot", new TrapperShoot(this));

        ChangeState("Idle");
    }

    public override void Start()
    {
        base.Start();
        InitializeStates();
    }
}

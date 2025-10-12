using UnityEngine;

public class Trooper : Enemy
{
    [Header("==Trooper Properties==")]
    public float distanceToShoot;
    public float distanceToMove;

    public override void InitializeStates()
    {
        AddState("Idle", new TrooperIdle(this));
        AddState("Move", new TrooperMove(this));
        AddState("Shoot", new TrooperShoot(this));

        ChangeState("Idle");
    }

    public override void Start()
    {
        base.Start();
        InitializeStates();
    }
}

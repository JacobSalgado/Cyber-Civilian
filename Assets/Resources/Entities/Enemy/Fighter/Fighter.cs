using UnityEngine;

public class Fighter : Enemy
{
    [Header("==Fighter Properties==")]
    public float distanceToHit;
    public float distanceToMove;

    public override void InitializeStates()
    {
        AddState("Idle", new FighterIdle(this));
        AddState("Move", new FighterMove(this));
        AddState("Hit", new FighterHit(this));

        ChangeState("Idle");
    }

    public override void Start()
    {
        base.Start();
        InitializeStates();
    }
}
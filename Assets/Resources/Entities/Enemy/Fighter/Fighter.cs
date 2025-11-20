using System;
using UnityEngine;

public class Fighter : Enemy
{
    [Header("==Fighter Properties==")]
    public float distanceToHit;
    public float distanceToMove;

    [NonSerialized] public float deltaCount = 0f;
    [NonSerialized] public int stepCounter = 1;

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
        type = EnemyTypes.MANTIS;
    }
}
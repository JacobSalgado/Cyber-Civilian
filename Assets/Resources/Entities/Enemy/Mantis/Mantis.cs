using System;
using UnityEngine;

public class Mantis : Enemy
{
    [Header("==Mantis Properties==")]
    public MantisPunch mantisPunch;
    public float distanceToMove;
    public float cooldownAfterPunch = 0.3f;

    public readonly float DISTANCE_TO_PUNCH = 2.5f;
    [NonSerialized] public float deltaCount = 0f;
    [NonSerialized] public int stepCounter = 1;

    public override void InitializeStates()
    {
        AddState("Idle", new MantisIdle(this));
        AddState("Move", new MantisMove(this));
        AddState("Punch", new MantisPunchAttack(this));

        ChangeState("Idle");
    }

    public override void Start()
    {
        base.Start();
        InitializeStates();
        type = EnemyTypes.MANTIS;
    }
}
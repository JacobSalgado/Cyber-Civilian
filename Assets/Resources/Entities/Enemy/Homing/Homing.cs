using UnityEngine;
using System;

public class Homing : Enemy
{
    [Header("==Homing Properties==")]
    public float distanceToShoot;
    public float distanceToMove;

    [NonSerialized] public float deltaCount = 0f;
    [NonSerialized] public int stepCounter = 1;

    public override void InitializeStates()
    {
        AddState("Idle", new HomingIdle(this));
        AddState("Move", new HomingMove(this));
        AddState("Shoot", new HomingShoot(this));

        ChangeState("Idle");
    }

    public override void Start()
    {
        base.Start();
        InitializeStates();
    }  
}

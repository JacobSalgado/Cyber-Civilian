using System;
using UnityEngine;

public class Missile : Projectile
{
    [NonSerialized] public Transform target;

    public override void InitializeStates()
    {
        AddState("Travel", new MissileTravel(this));

        ChangeState("Travel");
    }

    public override void Start()
    {
        base.Start();
        InitializeStates();
    }
}
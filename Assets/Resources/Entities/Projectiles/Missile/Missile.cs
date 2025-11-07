using UnityEngine;

public class Missile : Projectile
{
    public Transform target;

    public override void InitializeStates()
    {
        AddState("Idle", new MissileIdle(this));
        AddState("Travel", new MissileTravel(this));

        ChangeState("Travel");
    }

    public override void Start()
    {
        base.Start();
        InitializeStates();
    }
}

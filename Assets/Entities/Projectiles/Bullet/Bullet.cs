using UnityEngine;

public class Bullet : Projectile
{
    public override void InitializeStates()
    {
        AddState("Idle", new BulletIdle(this));
        AddState("Travel", new BulletTravel(this));

        ChangeState("Travel");
    }

    public override void Start()
    {
        base.Start();
        InitializeStates();
    }
}
using UnityEngine;

public class Bullet : Projectile
{
    public override void InitializeStates()
    {
        AddState("Idle", new BulletIdle(this));
        AddState("Travel", new BulletTravel(this));

        ChangeState("Idle");
    }

    public override void Start()
    {
        InitializeStates();
    }
}

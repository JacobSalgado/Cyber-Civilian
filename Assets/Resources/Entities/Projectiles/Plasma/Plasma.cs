using UnityEngine;
public class Plasma : Projectile
{
    public Collider2D damageFieldCollider;
    public override void InitializeStates()
    {
        AddState("Idle", new PlasmaIdle(this));
        AddState("Travel", new PlasmaTravel(this));

        ChangeState("Travel");
    }

    public override void Start()
    {
        base.Start();
        InitializeStates();
    }
}
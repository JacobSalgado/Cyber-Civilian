using UnityEngine;

public class Railgun : Projectile
{
    public override void InitializeStates()
    {
        AddState("Idle", new RailgunIdle(this));
        AddState("Travel", new RailgunTravel(this));

        ChangeState("Travel");
    }

    public override void Start()
    {
        base.Start();
        InitializeStates();
    }
}

using UnityEngine;

public class Railshot : Projectile
{
    public override void InitializeStates()
    {
        //AddState("Idle", new RailshotIdle(this));
        //AddState("Travel", new RailshotTravel(this));

        AddState("Active", new RailshotActive(this));

        ChangeState("Active");
    }

    public override void Start()
    {
        base.Start();
        InitializeStates();
    }
}
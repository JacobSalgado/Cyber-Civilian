using UnityEngine;

public class Railshot : Projectile
{

    public override void InitializeStates()
    {
        AddState("Active", new RailshotActive(this));

        ChangeState("Active");
    }

    public override void Start()
    {
        base.Start();
        InitializeStates();
    }
}

using UnityEngine;

public class Flame : Projectile
{
    

    public override void InitializeStates()
    {
        AddState("Idle", new FlameIdle(this));
        AddState("Travel", new FlameTravel(this));

        ChangeState("Travel");
    }

    public override void Start()
    {
        base.Start();
        InitializeStates();
    }
}

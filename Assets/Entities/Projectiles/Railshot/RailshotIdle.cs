using UnityEngine;

public class RailshotIdle : State
{
    readonly Railshot railshot;

    private float chargeTime = 0.75f;
    private float timer = 0f;

    public RailshotIdle(Entity new_entity) : base(new_entity)
    {
        railshot = (Railshot) new_entity;
    }

    public override void UpdateState()
    {
        railshot.ChangeState("Travel");
    }
}

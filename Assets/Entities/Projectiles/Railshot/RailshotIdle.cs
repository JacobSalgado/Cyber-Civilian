using UnityEngine;

public class RailshotIdle : State
{
    readonly Railshot railshot;

    public RailshotIdle(Entity new_entity) : base(new_entity)
    {
        railshot = (Railshot) new_entity;
    }

    public override void UpdateState()
    {
        railshot.ChangeState("Travel");
    }
}

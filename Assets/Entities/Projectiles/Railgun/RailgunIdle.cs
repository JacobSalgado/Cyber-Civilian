using UnityEngine;

public class RailgunIdle : State
{
    readonly Railgun railgun;

    public RailgunIdle(Entity new_entity) : base(new_entity)
    {
        railgun = (Railgun)new_entity;
    }

    public override void UpdateState()
    {
        railgun.ChangeState("Travel");
    }
}

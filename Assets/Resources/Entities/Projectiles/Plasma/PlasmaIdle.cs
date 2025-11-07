using UnityEngine;

public class PlasmaIdle : State
{
    readonly Plasma plasma;
    //private float deltaCount = 0f;

    public PlasmaIdle(Entity new_entity) : base(new_entity)
    {
        plasma = (Plasma) new_entity;
    }

    public override void UpdateState()
    {
        plasma.ChangeState("Travel");
    }
}

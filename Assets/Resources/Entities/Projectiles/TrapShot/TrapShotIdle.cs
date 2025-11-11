using UnityEngine;

public class TrapShotIdle : State
{
    readonly TrapShot trapShot;
    //private float deltaCount = 0f;

    public TrapShotIdle(Entity new_entity) : base(new_entity)
    {
        trapShot = (TrapShot) new_entity;
    }

    public override void UpdateState()
    {
        trapShot.ChangeState("Travel");
    }
}

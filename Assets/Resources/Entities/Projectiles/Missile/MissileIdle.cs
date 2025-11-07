using UnityEngine;

public class MissileIdle: State
{
    readonly Missile missile;

    public MissileIdle(Entity new_entity) : base(new_entity)
    {
        missile = (Missile)new_entity;
    }

    public override void UpdateState()
    {
        missile.ChangeState("Travel");
    }
}

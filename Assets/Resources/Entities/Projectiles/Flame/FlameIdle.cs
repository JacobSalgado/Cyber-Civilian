using UnityEngine;

public class FlameIdle : State
{
    readonly Flame flame;

    public FlameIdle(Entity new_entity) : base(new_entity)
    {
        flame = (Flame)new_entity;
    }
    
    public override void UpdateState()
    {
        flame.ChangeState("Travel");
    }
}
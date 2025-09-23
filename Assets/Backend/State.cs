using System.Collections.Generic;

public abstract class State
{
    public abstract void EnterState(Dictionary<string, object> args = null);
    public abstract void UpdateState();
    public abstract void ExitState(Dictionary<string, object> args = null);
    protected Entity entity;

    public State(Entity new_entity)
    {
        entity = new_entity;
    }
}

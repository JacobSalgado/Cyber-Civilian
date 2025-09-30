using System.Collections.Generic;

public abstract class State
{
    public virtual void EnterState(Dictionary<string, object> args = null)
    {
        
    }

    public virtual void UpdateState()
    {
        
    }

    public virtual void ExitState(Dictionary<string, object> args = null)
    {
        
    }

    public State(Entity new_entity) { }
    public State(GameManager gameManager) { }
}

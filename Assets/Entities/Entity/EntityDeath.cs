using System.Collections.Generic;
using UnityEngine;

public class EntityDeath : State
{
    public EntityDeath(Entity new_entity) : base(new_entity)
    {
        
    }

    public override void EnterState(Dictionary<string, object> args = null)
    {
        // TODO: handle death behavior here
    }

    public override void UpdateState()
    {

    }

    public override void ExitState(Dictionary<string, object> args = null)
    {
        
    }


}

using UnityEngine;
using System.Collections.Generic;

public class RailshotIdle : State
{
    readonly Railshot railshot;

    public RailshotIdle(Entity new_entity) : base(new_entity)
    {
        railshot = (Railshot)new_entity;
    }
    
    public override void EnterState(Dictionary<string, object> args = null)
    {
        
    }

    public override void UpdateState()
    {
        
    }
}
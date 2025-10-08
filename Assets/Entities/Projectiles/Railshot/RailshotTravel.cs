using System.Collections.Generic;
using UnityEngine;

public class RailshotTravel : State
{
    readonly Railshot railshot;
  
    public RailshotTravel(Entity new_entity) : base(new_entity)
    {
        railshot = (Railshot)new_entity; 
    }

    public override void EnterState(Dictionary<string, object> args = null)
    {
        //railgun.rigidBody.AddForce(railgun.transform.right * -1f * railgun.force, ForceMode2D.Impulse);
    }

    public override void UpdateState()
    {
        
    }
}

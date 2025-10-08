using System.Collections.Generic;
using UnityEngine;

public class RailgunTravel : State
{
    readonly Railgun railgun;
  
    public RailgunTravel(Entity new_entity) : base(new_entity)
    {
        railgun = (Railgun)new_entity; 
    }

    public override void EnterState(Dictionary<string, object> args = null)
    {
        railgun.rigidBody.AddForce(railgun.transform.right * -1f * railgun.force, ForceMode2D.Impulse);
    }

    public override void UpdateState()
    {
        
    }
}

using System.Collections.Generic;
using UnityEngine;

public class RailgunTravel : State
{
    readonly Railgun railgun;

    Vector2 rayOrigin;
    Vector2 rayDirection;
    float rayDistance;

    public RailgunTravel(Entity new_entity) : base(new_entity)
    {

    }

    public override void EnterState(Dictionary<string, object> args = null)
    {
        
    }

    public override void UpdateState()
    {
        
    }
}

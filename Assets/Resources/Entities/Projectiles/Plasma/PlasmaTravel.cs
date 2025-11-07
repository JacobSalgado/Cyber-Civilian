using System.Collections.Generic;
using UnityEngine;

public class PlasmaTravel : State
{
    readonly Plasma plasma;

    public PlasmaTravel(Entity new_entity) : base(new_entity)
    {
        plasma = (Plasma) new_entity;
    }

    public override void EnterState(Dictionary<string, object> args = null)
    {
        
    }

    public override void UpdateState()
    {
        plasma.rigidBody.linearVelocity = plasma.transform.right * -plasma.projData.moveSpeed;
    }
}

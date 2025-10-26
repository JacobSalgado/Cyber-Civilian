using System.Collections.Generic;
using UnityEngine;

public class HammerBase : State
{
    readonly HammerStrike hammerStrike;

    public HammerBase(Entity new_entity) : base(new_entity)
    {
        hammerStrike = (HammerStrike)new_entity;
    }

    public override void EnterState(Dictionary<string, object> args = null)
    {
        hammerStrike.rigidBody.linearVelocity = Vector2.zero;
    }

    public override void UpdateState()
    {
        
    }

    public override void ExitState(Dictionary<string, object> args = null)
    {
        
    }
}
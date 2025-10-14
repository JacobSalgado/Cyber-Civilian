using System.Collections.Generic;
using UnityEngine;

public class SniperHide : State
{
    readonly Sniper sniper;

    public SniperHide(Entity new_entity) : base(new_entity)
    {
        sniper = (Sniper)new_entity;
    }

    public override void EnterState(Dictionary<string, object> args = null)
    {

    }

    public override void UpdateState()
    {
        if (sniper.target == null)
        {
            Debug.LogError("Target not found");
            return;
        }

        // TODO: implement hiding behavior
    }
    
    public override void ExitState(Dictionary<string, object> args = null)
    {
        
    }
}
using System.Collections.Generic;
using UnityEngine;

public class SniperHide : State
{
    readonly Sniper sniper;
    private float hideTime;

    public SniperHide(Entity new_entity) : base(new_entity)
    {
        sniper = (Sniper)new_entity;
    }

    public override void EnterState(Dictionary<string, object> args = null)
    {
        sniper.timer = 0;
        sniper.Invisible();
        hideTime = Random.Range(sniper.invisibleTimeRange[0], sniper.invisibleTimeRange[1]);
    }

    public override void UpdateState()
    {
        if (sniper.target == null)
        {
            Debug.LogError("Target not found");
            return;
        }

        if (sniper.timer > hideTime)
            sniper.ChangeState("Idle");
    }
    
    public override void ExitState(Dictionary<string, object> args = null)
    {
        sniper.timer = 0;
        sniper.Visible();
    }
}
using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class CyberBossPunchAttack : State
{
    readonly CyberBoss cyberBoss;

    public CyberBossPunchAttack(Entity new_entity) : base(new_entity)
    {
        cyberBoss = (CyberBoss)new_entity;
    }

    public override void EnterState(Dictionary<string, object> args = null)
    {
        Debug.Log("In punch state");
        base.EnterState(args);
        cyberBoss.punch.EmitPunch();
        cyberBoss.PlayAnim("Punch");
    }

    public override void UpdateState()
    {
        if (!cyberBoss.punch.punchCollider.enabled)
        {
            cyberBoss.ChangeState("Travel");
        }
      
    }
}

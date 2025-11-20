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
        Vector2 punchDirection = cyberBoss.GetDirectionToPosition(cyberBoss.target.transform.position);
        
        Debug.Log("In punch state");
        base.EnterState(args);
        cyberBoss.punch.EmitPunch(punchDirection);
        cyberBoss.PlayAnim("Punch");
    }

    public override void UpdateState()
    {
        Debug.Log("in travel State");

        if (!cyberBoss.punch.punchCollider.enabled)
        {
            cyberBoss.ChangeState("Travel");
        }
      
    }
}

using UnityEngine;
using System.Collections.Generic;

public class CyberBossPunchAttack : State
{
    readonly CyberBoss cyberBoss;
    Vector2 punchDirection;

    public CyberBossPunchAttack(Entity new_entity) : base(new_entity)
    {
        cyberBoss = (CyberBoss)new_entity;
    }

    public override void EnterState(Dictionary<string, object> args = null)
    {
        punchDirection = cyberBoss.GetDirectionToPosition(cyberBoss.target.transform.position);
        cyberBoss.RotateToDirection(punchDirection);
        cyberBoss.punch.EmitPunch();
        Debug.Log("In punch state");
        //cyberBoss.PlayAnim("Punch");
    }

    public override void UpdateState()
    {
        cyberBoss.moveVelocity = cyberBoss.punchSpeed * punchDirection;
        cyberBoss.punch.punchRigidbody.linearVelocity = cyberBoss.punchSpeed * punchDirection;
        if (!cyberBoss.punch.punchCollider.enabled)
        {
            cyberBoss.ChangeState("Travel");
        }
    }

    public override void ExitState(Dictionary<string, object> args = null)
    {
        cyberBoss.moveVelocity = Vector2.zero;
        cyberBoss.punch.punchRigidbody.linearVelocity = Vector2.zero;
        cyberBoss.SetCooldown(1.2f);
    }
}

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
        // Activate Rage Mode
        if (cyberBoss.inRageMode)
        { 
            cyberBoss.punch.punchSpeed = 10f; 
        }

        punchDirection = cyberBoss.GetDirectionToPosition(cyberBoss.target.transform.position);
        cyberBoss.RotateToDirection(punchDirection);
        cyberBoss.punch.directionToPlayer = punchDirection;
        cyberBoss.punch.EmitPunch();
        Debug.Log("In punch state");
        cyberBoss.audioManager.PlayAudioSource("Punch");
        //cyberBoss.PlayAnim("Punch");
    }

    public override void UpdateState()
    {
        cyberBoss.moveVelocity = cyberBoss.punch.punchSpeed * punchDirection;
        if (!cyberBoss.punch.punchCollider.enabled)
        {
            cyberBoss.ChangeState("Travel");
        }
    }

    public override void ExitState(Dictionary<string, object> args = null)
    {
        cyberBoss.moveVelocity = Vector2.zero;
        cyberBoss.SetCooldown(0.5f);
    }
}

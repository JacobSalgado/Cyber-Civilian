using UnityEngine;
using System.Collections.Generic;

public class CyberBossStompAttack: State
{
    readonly CyberBoss cyberBoss;

    public CyberBossStompAttack(Entity new_entity) : base(new_entity)
    {
        cyberBoss = (CyberBoss) new_entity;
    }

    public override void EnterState(Dictionary<string, object> args = null)
    {
        Debug.Log("In Stomp state");
        cyberBoss.moveVelocity = Vector2.zero;
        cyberBoss.stomp.directionToPlayer = cyberBoss.GetDirectionToPosition(cyberBoss.target.position);
        cyberBoss.stomp.startScale = cyberBoss.stomp.gameObject.transform.localScale;
        cyberBoss.stomp.EmitPush(cyberBoss.transform.position, cyberBoss.stomp.directionToPlayer);
        cyberBoss.audioManager.PlayAudioSource("Stomp");
        cyberBoss.PlayAnim("Stomp");
    }

    public override void UpdateState()
    {
        if (!cyberBoss.stomp.aoeCollider.enabled)
        {
            cyberBoss.ChangeState("Travel");
        }
    }

    public override void ExitState(Dictionary<string, object> args = null)
    {
        cyberBoss.SetCooldown(4.0f);
        cyberBoss.stomp.gameObject.transform.localScale = cyberBoss.stomp.startScale;
    }

}

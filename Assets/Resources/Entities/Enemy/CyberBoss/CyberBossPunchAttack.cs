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
        base.EnterState(args);
        cyberBoss.PlayAnim("Punch");
    }

    public override void UpdateState()
    {
        if (cyberBoss.target == null)
        {
            Debug.Log("Target not found");
            return;
        }

        cyberBoss.rigidBody.linearVelocity = Vector2.zero;

        float distance = cyberBoss.GetDistanceToTarget();
        if (distance < cyberBoss.distanceToHit)
        {
            Vector2 dir = cyberBoss.GetDirectionToPosition(cyberBoss.target.gameObject.transform.position);
            cyberBoss.RotateToDirection(dir);
            cyberBoss.ShootWeapon(cyberBoss.weapon, null, cyberBoss.firePoint, 7);
        }
        else if (distance < cyberBoss.distanceToMove)
        {
            cyberBoss.ChangeState("Travel");
        }
        else
        {
            cyberBoss.ChangeState("Travel");
        }
    }

    /*private IEnumerator MoveAfterDelay()
    {
        yield return new WaitForSeconds(0.5f);
        cyberBoss.ChangeState("Move");
    }*/
}

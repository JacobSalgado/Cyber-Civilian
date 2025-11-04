using System.Collections.Generic;
using System.Collections;
using UnityEngine;

public class FighterHit : State
{
    readonly Fighter fighter;

    public FighterHit(Entity new_entity) : base(new_entity)
    {
        fighter = (Fighter)new_entity;
    }

    public override void EnterState(Dictionary<string, object> args = null)
    {
        fighter.rigidBody.linearVelocity = Vector2.zero;
    }

    public override void UpdateState()
    {
        if (fighter.target == null)
        {
            Debug.LogError("Target not found");
            return;
        }

        fighter.rigidBody.linearVelocity = Vector2.zero;

        float distance = fighter.GetDistanceToTarget();
        if (distance < fighter.distanceToHit)
        {
            Vector2 dir = fighter.GetDirectionToPosition(fighter.target.gameObject.transform.position);
            fighter.RotateToDirection(dir);
            fighter.ShootWeapon(fighter.weapon, null, fighter.firePoint, 7);
        }
        else if (distance < fighter.distanceToMove)
            fighter.StartCoroutine(MoveAfterDelay());
        else
            fighter.ChangeState("Idle");
    }

    private IEnumerator MoveAfterDelay()
    {
        yield return new WaitForSeconds(0.5f);
        fighter.ChangeState("Move");
    }

    public override void ExitState(Dictionary<string, object> args = null)
    {

    }
}
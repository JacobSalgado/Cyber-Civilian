using System.Collections.Generic;
using System.Collections;
using UnityEngine;

public class FighterIdle : State
{
    readonly Fighter fighter;

    public FighterIdle(Entity new_entity) : base(new_entity)
    {
        fighter = (Fighter) new_entity;
    }

    public override void EnterState(Dictionary<string, object> args = null)
    {
        fighter.rigidBody.linearVelocity = Vector2.zero;
        fighter.deltaCount = 0f;
        fighter.stepCounter = 1;
    }

    public override void UpdateState()
    {
        if (fighter.target == null)
        {
            Debug.LogError("Target not found");
            return;
        }

        fighter.rigidBody.linearVelocity = Vector2.zero;

        float distanceToTarget = fighter.GetDistanceToTarget();
        if (distanceToTarget <= -1f)
        { // assertion check
            Debug.Log("target not set");
            return;
        }

        if (distanceToTarget < fighter.distanceToHit)
            fighter.ChangeState("Hit");
        else if (distanceToTarget < fighter.distanceToMove)
            fighter.StartCoroutine(MoveAfterDelay());

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

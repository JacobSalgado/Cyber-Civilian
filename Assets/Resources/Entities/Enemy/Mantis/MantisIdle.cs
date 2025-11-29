using System.Collections.Generic;
using System.Collections;
using UnityEngine;

public class MantisIdle : State
{
    readonly Mantis mantis;

    public MantisIdle(Entity new_entity) : base(new_entity)
    {
        mantis = (Mantis) new_entity;
    }

    public override void EnterState(Dictionary<string, object> args = null)
    {
        mantis.moveVelocity = Vector2.zero;        
        mantis.deltaCount = 0f;
        mantis.stepCounter = 1;
        mantis.PlayAnim("Idle");
    }

    public override void UpdateState()
    {
        if (mantis.target == null)
        {
            Debug.LogError("Target not found");
            return;
        }

        mantis.moveVelocity = Vector2.zero;

        float distanceToTarget = mantis.GetDistanceToTarget();
        if (distanceToTarget <= -1f)
        { // assertion check
            Debug.Log("target not set");
            return;
        }

        if (distanceToTarget < mantis.DISTANCE_TO_PUNCH)
            mantis.ChangeState("Punch");
        else if (distanceToTarget < mantis.distanceToMove)
            mantis.StartCoroutine(MoveAfterDelay());

    }
    
    private IEnumerator MoveAfterDelay()
    {
        yield return new WaitForSeconds(0.5f);
        mantis.ChangeState("Move");
    }

    public override void ExitState(Dictionary<string, object> args = null)
    {
        
    }
}

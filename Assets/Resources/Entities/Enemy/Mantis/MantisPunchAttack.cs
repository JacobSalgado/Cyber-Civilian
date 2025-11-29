using System.Collections.Generic;
using System.Collections;
using UnityEngine;

public class MantisPunchAttack : State
{
    const float PUNCH_TIME = 0.11f;
    readonly Mantis mantis;
    float timer = 0f;
    float distance = 0f;
    bool cooldownFlag = false;
    bool punchThrown = false;

    public MantisPunchAttack(Entity new_entity) : base(new_entity)
    {
        mantis = (Mantis)new_entity;
    }

    public override void EnterState(Dictionary<string, object> args = null)
    {
        timer = 0f;
        mantis.RotateToDirection(mantis.GetDirectionToPosition(mantis.target.gameObject.transform.position));
        mantis.PlayAnim("Punch");
        // play audio
    }

    public override void UpdateState()
    {
        // TODO - possibly include dashing quickly at the player (can be added during rage mode)

        timer += Time.deltaTime;

        if (mantis.target == null)
        {
            Debug.LogError("Target not found");
            return;
        }

        if (!punchThrown && timer > PUNCH_TIME)
        {
            mantis.mantisPunch.EmitPunch();
            
            punchThrown = true;
        }

        distance = mantis.GetDistanceToTarget();
        mantis.moveVelocity = Vector2.zero;

        if (!cooldownFlag && punchThrown && !mantis.mantisPunch.punchCollider.enabled)
        {
            cooldownFlag = true;
            mantis.StartCoroutine(MoveAfterDelay());
        }
    }

    private IEnumerator MoveAfterDelay()
    {
        yield return new WaitForSeconds(mantis.cooldownAfterPunch);
        if (distance < mantis.DISTANCE_TO_PUNCH)
            mantis.ChangeState("Punch");
        if (distance < mantis.distanceToMove) 
            mantis.ChangeState("Move");
        else 
            mantis.ChangeState("Idle");
        
    }

    public override void ExitState(Dictionary<string, object> args = null)
    {
        punchThrown = false;
        cooldownFlag = false;
    }
}
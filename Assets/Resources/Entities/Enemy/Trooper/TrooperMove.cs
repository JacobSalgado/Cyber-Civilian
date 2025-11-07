using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// TrooperMove:
/// <para>- move towards the target until it's within distanceToShoot range</para>
/// <para>- if target is outside the distanceToMove range, change to Idle state</para>
/// </summary>
public class TrooperMove : State
{
    private const float timeToStep = 0.3f;

    readonly Trooper trooper;
    private Vector2 directionToTarget;
    private float deltaCount = 0f;
    int stepCounter = 1;

    public TrooperMove(Entity new_entity) : base(new_entity)
    {
        trooper = (Trooper)new_entity;
    }

    public override void EnterState(Dictionary<string, object> args = null)
    {
        deltaCount = 0f;
        stepCounter = 1;
        trooper.audioManager.PlayAudioSource($"Footsteps{stepCounter++}");
        trooper.PlayAnim("Walk");
    }

    public override void UpdateState()
    {
        if (trooper.target == null)
        {
            Debug.LogError("Target not found");
            return;
        }

        deltaCount += Time.deltaTime;

        if (trooper.PlayFootsteps(deltaCount, timeToStep, stepCounter))
        {
            deltaCount = 0f;
            stepCounter++;
            if (stepCounter > 3) stepCounter = 1;
        }

        float distance = trooper.GetDistanceToTarget();
        if (distance > -1f)
        {
            if (distance < trooper.distanceToShoot)
            {
                trooper.ChangeState("Shoot");
                return;
            }
            else if (distance < trooper.distanceToMove)
            {
                directionToTarget = trooper.GetDirectionToPosition(trooper.target.transform.position);
                trooper.RotateToDirection(directionToTarget);
                trooper.rigidBody.linearVelocity = directionToTarget * trooper.entityData.moveSpeed;
            }
            else trooper.ChangeState("Idle");
        }
    }

    public override void ExitState(Dictionary<string, object> args = null)
    {
        
    }
}

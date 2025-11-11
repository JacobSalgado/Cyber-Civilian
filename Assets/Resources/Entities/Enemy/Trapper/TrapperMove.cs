using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// TrapperMove:
/// <para>- move towards the target until it's within distanceToShoot range</para>
/// <para>- if target is outside the distanceToMove range, change to Idle state</para>
/// </summary>
public class TrapperMove : State
{
    private const float timeToStep = 0.3f;

    readonly Trapper trapper;
    private Vector2 directionToTarget;
    private float deltaCount = 0f;
    int stepCounter = 1;

    public TrapperMove(Entity new_entity) : base(new_entity)
    {
        trapper = (Trapper)new_entity;
    }

    public override void EnterState(Dictionary<string, object> args = null)
    {
        deltaCount = 0f;
        stepCounter = 1;
        trapper.audioManager.PlayAudioSource($"Footsteps{stepCounter++}");
        trapper.PlayAnim("Walk");
    }

    public override void UpdateState()
    {
        if (trapper.target == null)
        {
            Debug.LogError("Target not found");
            return;
        }

        deltaCount += Time.deltaTime;

        if (trapper.PlayFootsteps(deltaCount, timeToStep, stepCounter))
        {
            deltaCount = 0f;
            stepCounter++;
            if (stepCounter > 3) stepCounter = 1;
        }

        float distance = trapper.GetDistanceToTarget();
        if (distance > -1f)
        {
            if (distance < trapper.distanceToShoot)
            {
                trapper.ChangeState("Shoot");
                return;
            }
            else if (distance < trapper.distanceToMove)
            {
                directionToTarget = trapper.GetDirectionToPosition(trapper.target.transform.position);
                trapper.RotateToDirection(directionToTarget);
                trapper.rigidBody.linearVelocity = directionToTarget * trapper.entityData.moveSpeed;
            }
            else trapper.ChangeState("Idle");
        }
    }

    public override void ExitState(Dictionary<string, object> args = null)
    {
        
    }
}

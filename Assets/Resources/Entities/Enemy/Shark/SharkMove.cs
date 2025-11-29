using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// SharkMove:
/// <para>- move towards the target until it's within distanceToShoot range</para>
/// <para>- if target is outside the distanceToMove range, change to Idle state</para>
/// </summary>
public class SharkMove : State
{
    private const float timeToStep = 0.3f;

    readonly Shark shark;
    private Vector2 directionToTarget;
    private float deltaCount = 0f;
    int stepCounter = 1;

    public SharkMove(Entity new_entity) : base(new_entity)
    {
        shark = (Shark)new_entity;
    }

    public override void EnterState(Dictionary<string, object> args = null)
    {
        deltaCount = 0f;
        stepCounter = 1;
        shark.audioManager.PlayAudioSource($"Footsteps{stepCounter++}");
        shark.PlayAnim("Walk");
    }

    public override void UpdateState()
    {
        if (shark.target == null)
        {
            Debug.LogError("Target not found");
            return;
        }

        deltaCount += Time.deltaTime;

        if (shark.PlayFootsteps(deltaCount, timeToStep, stepCounter))
        {
            deltaCount = 0f;
            stepCounter++;
            if (stepCounter > 3) stepCounter = 1;
        }

        float distance = shark.GetDistanceToTarget();
        if (distance > -1f)
        {
            if (distance < shark.distanceToShoot)
            {
                shark.ChangeState("Shoot");
                return;
            }
            else if (distance < shark.distanceToMove)
            {
                directionToTarget = shark.GetDirectionToPosition(shark.target.transform.position);
                shark.RotateToDirection(directionToTarget);
                shark.moveVelocity = directionToTarget * shark.entityData.moveSpeed;
            }
            else shark.ChangeState("Idle");
        }
    }

    public override void ExitState(Dictionary<string, object> args = null)
    {
        
    }
}

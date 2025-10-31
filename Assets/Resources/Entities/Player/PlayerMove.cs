using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : State
{
    private const float timeToStep = 0.25f;

    Vector2 velocity;
    readonly Player player;
    private float deltaCount = 0f;
    int stepCounter = 1;

    public PlayerMove(Entity new_entity) : base(new_entity)
    {
        player = (Player) new_entity;
    }

    public override void EnterState(Dictionary<string, object> args = null)
    {
        deltaCount = 0f;
        stepCounter = 1;
        player.audioManager.PlayAudioSource($"Footsteps{stepCounter++}");
    }

    public override void UpdateState()
    {
        velocity = player.moveAction.action.ReadValue<Vector2>();

        if (velocity != Vector2.zero)
        {
            deltaCount += Time.deltaTime;
            player.rigidBody.linearVelocity = velocity * player.entityData.moveSpeed;
            if (player.PlayFootsteps(deltaCount, timeToStep, stepCounter))
            {
                deltaCount = 0f;
                stepCounter++;
                if (stepCounter > 3) stepCounter = 1;
            }
        }
        else player.ChangeState("Idle");
    }

    public override void ExitState(Dictionary<string, object> args = null)
    {

    }

}

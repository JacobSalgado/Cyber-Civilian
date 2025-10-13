using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : State
{
    private const float timeToStep = 0.35f;

    Vector2 velocity;
    readonly Player player;
    private float deltaCount = 0f;
    int stepCounter = 1;

    public PlayerMove(Entity new_entity) : base(new_entity)
    {
        player = (Player) new_entity;
    }

    private void PlayFootsteps()
    {
        if (deltaCount > timeToStep)
        {
            player.PlaySFX($"Footstep{stepCounter++}");
            deltaCount = 0f;
            if (stepCounter > 3) stepCounter = 1;
        }
    }

    public override void EnterState(Dictionary<string, object> args = null)
    {
        deltaCount = 0f;
        stepCounter = 1;
        player.PlaySFX($"Footstep{stepCounter++}");
    }

    public override void UpdateState()
    {
        velocity = player.moveAction.action.ReadValue<Vector2>();

        if (velocity != Vector2.zero)
        {
            deltaCount += Time.deltaTime;
            player.rigidBody.linearVelocity = velocity * player.entityData.moveSpeed;
            PlayFootsteps();
        }
        else player.ChangeState("Idle");
    }

    public override void ExitState(Dictionary<string, object> args = null)
    {

    }

}

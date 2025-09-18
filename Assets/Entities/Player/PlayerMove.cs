using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : State
{
    Vector2 velocity;
    Player player;
    private float deltaCount = 0f;
    private float timeToStep = 0.35f;
    int stepCounter = 0;

    public PlayerMove(Entity new_entity) : base(new_entity)
    {
        player = (Player)new_entity;
    }

    private void PlayFootsteps()
    {
        if (deltaCount > timeToStep)
        {
            player.SFXPlayer.PlayOneShot(player.entityData.SFX[stepCounter++]);
            deltaCount = 0f;
            if (stepCounter > 2) stepCounter = 0;
        }
    }

    public override void EnterState(Dictionary<string, object> args = null)
    {
        deltaCount = 0f;
        stepCounter = 0;
        player.SFXPlayer.PlayOneShot(player.entityData.SFX[stepCounter++]);
    }

    public override void UpdateState()
    {
        velocity = player.moveAction.action.ReadValue<Vector2>();

        if (velocity != Vector2.zero)
        {
            deltaCount += Time.deltaTime;
            entity.rb.linearVelocity = new Vector2(velocity.x * entity.entityData.moveSpeed, velocity.y * entity.entityData.moveSpeed);
            PlayFootsteps();
        }
        else entity.ChangeState("Idle");
    }

    public override void ExitState(Dictionary<string, object> args = null)
    {

    }

}

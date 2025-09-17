using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMove : State
{
    Vector2 velocity;
    Player player;

    public PlayerMove(Entity new_entity) : base(new_entity)
    {
        player = (Player) new_entity;
    }

    public override void EnterState(Dictionary<string, object> args = null)
    {
        
    }

    public override void UpdateState()
    {
        velocity = player.moveAction.action.ReadValue<Vector2>();

        if (player.moveAction.action.ReadValue<Vector2>() != Vector2.zero)
            entity.rb.linearVelocity = new Vector2(velocity.x * entity.entityData.moveSpeed, velocity.y * entity.entityData.moveSpeed);
        else
            entity.ChangeState("Idle");
    }

    public override void ExitState(Dictionary<string, object> args = null)
    {
        
    }
}

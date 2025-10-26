using System.Collections.Generic;
using UnityEngine;

public class PlayerIdle : State
{
    readonly Player player;

    public PlayerIdle(Entity new_entity) : base(new_entity)
    {
        player = (Player) new_entity;
    }

    public override void EnterState(Dictionary<string, object> args = null)
    {
        player.rigidBody.linearVelocity = Vector2.zero;
    }

    public override void UpdateState()
    {
        if (player.moveAction.action.ReadValue<Vector2>() != Vector2.zero)
            player.ChangeState("Move");
    }

    public override void ExitState(Dictionary<string, object> args = null)
    {
        
    }
}

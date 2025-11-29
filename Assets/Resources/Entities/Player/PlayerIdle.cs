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
        player.moveVelocity = Vector2.zero;
    }

    public override void UpdateState()
    {
        player.moveVelocity = Vector2.zero;
        if (player.IsMoving())
            player.ChangeState("Move");
    }

    public override void ExitState(Dictionary<string, object> args = null)
    {
        
    }
}

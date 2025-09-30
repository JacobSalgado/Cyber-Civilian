using System.Collections.Generic;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.XR;

public class PlayerDash : State
{
    Vector2 velocity;
    readonly Player player;

    public PlayerDash(Entity new_entity) : base(new_entity)
    {
        player = (Player)new_entity;
    }

    public override void EnterState(Dictionary<string, object> args = null)
    {
        player.StartCoroutine(initiateDash());
    }

    private IEnumerator initiateDash()
    {
        yield return player.StartCoroutine(Dash());
        player.ChangeState("Idle");
    }
    private IEnumerator Dash()
    {
        player.setCanDash(false);
        player.setIsDashing(true);
        velocity = player.moveAction.action.ReadValue<Vector2>();
        player.rigidBody.linearVelocity = new Vector2(velocity.x * player.dashPower, velocity.y * player.dashPower);
        player.tr.emitting = true;
        yield return new WaitForSeconds(player.dashTime);
        player.tr.emitting = false;
        player.setIsDashing(false);
        yield return new WaitForSeconds(player.dashCooldown);
        player.setCanDash(true);
    }

    public override void UpdateState()
    {

    }

    public override void ExitState(Dictionary<string, object> args = null)
    {

    }
}

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
        velocity = player.moveAction.action.ReadValue<Vector2>();
        player.tr.emitting = true;
        player.StartCoroutine(initiateDash());
    }

    private IEnumerator initiateDash()
    {
        yield return player.StartCoroutine(Dash());
        player.ChangeState("Move");
    }
    private IEnumerator Dash()
    {
        // velocity = player.moveAction.action.ReadValue<Vector2>();
        // player.rigidBody.linearVelocity = new Vector2(velocity.normalized.x * player.dashPower, velocity.normalized.y * player.dashPower);
        // player.rigidBody.linearVelocity = velocity.normalized * player.dashPower * player.entityData.moveSpeed;
        // player.tr.emitting = true;
        yield return new WaitForSeconds(player.dashTime);
        player.tr.emitting = false;
        player.setIsDashing(false);
        player.StartCoroutine(DashCooldown());
        // yield return new WaitForSeconds(player.dashCooldown);
        // player.setCanDash(true);
    }

    private IEnumerator DashCooldown()
    {
        yield return new WaitForSeconds(player.dashCooldown);
        player.setCanDash(true);
    }

    public override void UpdateState()
    {
        if (player.getIsDashing())
        {
            player.rigidBody.linearVelocity = velocity.normalized * player.dashPower * player.entityData.moveSpeed;
            return;
        }
    }

    public override void ExitState(Dictionary<string, object> args = null)
    {

    }
}

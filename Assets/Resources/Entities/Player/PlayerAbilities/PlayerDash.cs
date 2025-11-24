using System.Collections.Generic;
using System.Collections;
using UnityEngine;

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
        player.moveVelocity = Vector2.zero;
        velocity = player.moveAction.action.ReadValue<Vector2>();
        player.tr.emitting = true;
        player.isDashing = true;
        player.StartCoroutine(InitiateDash());
    }

    public override void UpdateState()
    {
        player.moveVelocity = player.dashPower * player.entityData.moveSpeed * velocity.normalized;
    }

    public override void ExitState(Dictionary<string, object> args = null)
    {

    }

    private IEnumerator InitiateDash()
    {
        player.audioManager.PlayAudioSource("Dashing");
        yield return player.StartCoroutine(Dash());
    }
    private IEnumerator Dash()
    {
        yield return new WaitForSeconds(player.dashTime);
        player.tr.emitting = false;
        player.invincibility = false;
        player.ChangeState("Idle");
        player.StartCoroutine(DashCooldown());
    }

    private IEnumerator DashCooldown()
    {
        yield return new WaitForSeconds(player.dashCooldown);
        player.isDashing = false;
    }
}

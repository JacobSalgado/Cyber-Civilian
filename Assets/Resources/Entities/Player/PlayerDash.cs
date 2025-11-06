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
        player.rigidBody.linearVelocity = Vector2.zero;
        velocity = player.moveAction.action.ReadValue<Vector2>();
        player.tr.emitting = true;
        player.StartCoroutine(initiateDash());
    }

    public override void UpdateState()
    {
        if (player.getIsDashing())
        {
            player.rigidBody.linearVelocity = player.dashPower * player.entityData.moveSpeed * velocity.normalized;
            return;
        }
    }

    public override void ExitState(Dictionary<string, object> args = null)
    {

    }

    private IEnumerator initiateDash()
    {
        player.audioManager.PlayAudioSource("Dashing");
        yield return player.StartCoroutine(Dash());
        player.ChangeState("Move");
    }
    private IEnumerator Dash()
    {
        yield return new WaitForSeconds(player.dashTime);
        player.tr.emitting = false;
        player.setIsDashing(false);
        player.invincibility = false;
        player.StartCoroutine(DashCooldown());
    }

    private IEnumerator DashCooldown()
    {
        yield return new WaitForSeconds(player.dashCooldown);
        player.setCanDash(true);
    }
}

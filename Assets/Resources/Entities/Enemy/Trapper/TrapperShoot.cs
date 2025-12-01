using System.Collections.Generic;
using System.Collections;
using UnityEngine;

public class TrapperShoot : State
{
    private const float minimumShootTime = 1.9f;

    readonly Trapper trapper;
    private float timer = 0f;

    public TrapperShoot(Entity new_entity) : base(new_entity)
    {
        trapper = (Trapper)new_entity;
    }

    public override void EnterState(Dictionary<string, object> args = null)
    {
        trapper.moveVelocity = Vector2.zero;
        timer = 0f;
        trapper.PlayAnim("Idle");
    }

    public override void UpdateState()
    {
        if (trapper.target == null)
        {
            Debug.LogError("Target not found");
            return;
        }

        trapper.moveVelocity = Vector2.zero;
        timer += Time.deltaTime;

        float distance = trapper.GetDistanceToTarget();
        if (distance < trapper.distanceToShoot)
        {
            Vector2 dir = trapper.GetDirectionToPosition(trapper.target.gameObject.transform.position);
            trapper.RotateToDirection(dir);
            trapper.ShootWeapon(trapper.weapon, null, trapper.firePoint, 7);
        }
        else if (timer > minimumShootTime)
        {
            if (distance < trapper.distanceToMove)
                trapper.ChangeState("Move");
            else
                trapper.ChangeState("Idle");
        }

        if (LevelManager.player.isSlowed)
        {
            trapper.StartCoroutine(DelayAfterSlow());
        }
    }

    private IEnumerator DelayAfterSlow()
    {
        yield return new WaitForSeconds(trapper.delayAfterSlow);
        trapper.ChangeState("Idle");
    }
    
    public override void ExitState(Dictionary<string, object> args = null)
    {

    }
}
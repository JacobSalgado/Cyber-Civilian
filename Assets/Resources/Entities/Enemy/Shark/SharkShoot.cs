using System.Collections.Generic;
using UnityEngine;

public class SharkShoot : State
{
    private const float minimumShootTime = 1.9f;

    readonly Shark shark;
    private float timer = 0f;

    public SharkShoot(Entity new_entity) : base(new_entity)
    {
        shark = (Shark)new_entity;
    }

    public override void EnterState(Dictionary<string, object> args = null)
    {
        shark.moveVelocity = Vector2.zero;
        timer = 0f;
        shark.PlayAnim("Idle");
    }

    public override void UpdateState()
    {
        if (shark.target == null)
        {
            Debug.LogError("Target not found");
            return;
        }

        shark.moveVelocity = Vector2.zero;
        timer += Time.deltaTime;

        float distance = shark.GetDistanceToTarget();
        if (distance < shark.distanceToShoot)
        {
            Vector2 dir = shark.GetDirectionToPosition(shark.target.gameObject.transform.position);
            shark.RotateToDirection(dir);
            shark.ShootWeapon(shark.weapon, null, shark.firePoint, 7);
        }
        else if (timer > minimumShootTime)
        {
            if (distance < shark.distanceToMove)
                shark.ChangeState("Move");
            else
                shark.ChangeState("Idle");
        }
    }

    public override void ExitState(Dictionary<string, object> args = null)
    {

    }
}
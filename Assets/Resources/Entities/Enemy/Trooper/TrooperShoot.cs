using System.Collections.Generic;
using UnityEngine;

public class TrooperShoot : State
{
    private const float minimumShootTime = 1.9f;

    readonly Trooper trooper;
    private float timer = 0f;

    public TrooperShoot(Entity new_entity) : base(new_entity)
    {
        trooper = (Trooper)new_entity;
    }

    public override void EnterState(Dictionary<string, object> args = null)
    {
        trooper.rigidBody.linearVelocity = Vector2.zero;
        trooper.rigidBody.angularVelocity = 0f;
        timer = 0f;
    }

    public override void UpdateState()
    {
        if (trooper.target == null)
        {
            Debug.LogError("Target not found");
            return;
        }

        trooper.rigidBody.linearVelocity = Vector2.zero;
        trooper.rigidBody.angularVelocity = 0f;
        timer += Time.deltaTime;

        float distance = trooper.GetDistanceToTarget();
        if (distance < trooper.distanceToShoot)
        {
            Vector2 dir = trooper.GetDirectionToPosition(trooper.target.gameObject.transform.position);
            trooper.RotateToDirection(dir);
            trooper.ShootWeapon(trooper.weapon, null, trooper.firePoint, 7);
        }
        else if (timer > minimumShootTime)
        {
            if (distance < trooper.distanceToMove)
                trooper.ChangeState("Move");
            else
                trooper.ChangeState("Idle");
        }
    }

    public override void ExitState(Dictionary<string, object> args = null)
    {

    }
}
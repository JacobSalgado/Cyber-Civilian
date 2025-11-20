using UnityEngine;
using System;
using System.Threading;

public class HomingShoot : State
{
    readonly Homing homing;

    private const float timeToShoot = 0.35f;
    private const float minimumShootTime = 1.9f;
    private float timer;
    private float shootTimer = 0f;

    public HomingShoot(Entity new_entity) : base(new_entity)
    {
        homing = (Homing)new_entity;
        timer = 0f;
        shootTimer = 0f;
    }

    public override void UpdateState()
    {
        if (homing.target == null)
        {
            Debug.Log("Target not found");
            return;
        }

        homing.moveVelocity = Vector2.zero;
        shootTimer += Time.deltaTime;
        timer += Time.deltaTime;

        float distanceToTarget = homing.GetDistanceToTarget();
        if (distanceToTarget < homing.distanceToShoot)
        {
            Vector2 dir = homing.GetDirectionToPosition(homing.target.gameObject.transform.position);
            homing.RotateToDirection(dir);

            if (shootTimer > timeToShoot)
            {
                homing.ShootWeapon(homing.weapon, null, homing.firePoint, 7);
                shootTimer = 0f;
            }
        }
        else if (timer > minimumShootTime)
        {
            if (distanceToTarget < homing.distanceToMove)
            {
                homing.ChangeState("Move");
            }
            else
            {
                homing.ChangeState("Idle");
            }
        }
    }
}

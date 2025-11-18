using UnityEngine;
using System;
using System.Threading;

public class HomingShoot : State
{
    readonly Homing homing;
    private readonly Weapon homingMissile;

    private const float minimumShootTime = 1.9f;
    private float timer;

    public HomingShoot(Entity new_entity) : base(new_entity)
    {
        homing = (Homing)new_entity;
        homingMissile = homing.weapon.GetComponent<Weapon>();
        timer = 0f;
        // Play Anim here
    }

    public override void UpdateState()
    {
        if (homing.target == null)
        {
            Debug.Log("Target not found");
            return;
        }

        homing.rigidBody.linearVelocity = Vector2.zero;
        timer += Time.deltaTime;

        float distanceToTarget = homing.GetDistanceToTarget();

        if (distanceToTarget < homing.distanceToShoot)
        {
            Vector2 dir = homing.GetDirectionToPosition(homing.target.gameObject.transform.position);
            homing.RotateToDirection(dir);

            homing.ShootWeapon(homing.weapon, null, homing.firePoint, 7);
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

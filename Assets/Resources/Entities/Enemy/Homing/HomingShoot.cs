using UnityEngine;
using System;

public class HomingShoot : State
{
    readonly Homing homing;
    private readonly Weapon homingMissile;

    public HomingShoot(Entity new_entity) : base(new_entity)
    {
        homing = (Homing)new_entity;
        homingMissile = homing.weapon.GetComponent<Weapon>();
    }

    public override void UpdateState()
    {
        if (homing.target == null)
        {
            Debug.Log("Target not found");
            return;
        }
        
        float distanceToTarget = homing.GetDistanceToTarget();

        if (distanceToTarget < homing.distanceToShoot)
        {
            Vector2 dir = homing.GetDirectionToPosition(homing.target.gameObject.transform.position);
            homing.RotateToDirection(dir);

            homing.ShootWeapon(homing.weapon, null, homing.firePoint, 7);
        }

        homing.ShootWeapon(homing.weapon, null, homing.firePoint, 7);
    }
}

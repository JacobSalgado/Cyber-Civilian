using System.Collections.Generic;
using UnityEngine;

public class SniperShoot : State
{
    readonly Sniper sniper;
    private readonly Weapon sniperRifle;
    private float directionTimer = 0f;
    private float stopRotatingTime = 0f;

    public SniperShoot(Entity new_entity) : base(new_entity)
    {
        sniper = (Sniper)new_entity;
        sniperRifle = sniper.weapon.GetComponent<Weapon>();
        stopRotatingTime = sniperRifle.projData.timeToSpawn - 0.45f;
    }

    public override void EnterState(Dictionary<string, object> args = null)
    {
        directionTimer = 0f;
    }

    public override void UpdateState()
    {
        if (sniper.target == null)
        {
            Debug.LogError("Target not found");
            return;
        }

        directionTimer += Time.deltaTime;

        sniper.rigidBody.linearVelocity = Vector2.zero;

        float distanceToTarget = sniper.GetDistanceToTarget();
        if (distanceToTarget < sniper.distanceToHide && !sniper.isInvisibleRecharging && sniper.timer > sniper.invisibleRechargeTime)
        {
            sniper.ChangeState("Hide");
            return;
        }

        if (distanceToTarget < sniper.distanceToShoot)
        {
            if (directionTimer < stopRotatingTime)
            {
                Vector2 dir = sniper.GetDirectionToPosition(sniper.target.gameObject.transform.position);
                sniper.RotateToDirection(dir);
            }

            sniper.ShootWeapon(sniper.weapon, null, sniper.firePoint, 7);
        }

        if (sniperRifle.fireTimer == 0f)
            directionTimer = 0f;
    }
    
    public override void ExitState(Dictionary<string, object> args = null)
    {
        
    }
}
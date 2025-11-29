using System.Collections.Generic;
using UnityEngine;

public class CrabShoot : State
{
    readonly Crab crab;
    private readonly Weapon crabRifle;
    private float directionTimer = 0f;
    private readonly float stopRotatingTime = 0f;

    public CrabShoot(Entity new_entity) : base(new_entity)
    {
        crab = (Crab)new_entity;
        crabRifle = crab.weapon.GetComponent<Weapon>();
        stopRotatingTime = crabRifle.projData.timeToSpawn - 0.45f;
    }

    public override void EnterState(Dictionary<string, object> args = null)
    {
        directionTimer = 0f;
        crab.PlayAnim("Attack");
    }

    public override void UpdateState()
    {
        if (crab.target == null)
        {
            Debug.LogError("Target not found");
            return;
        }

        directionTimer += Time.deltaTime;

        crab.moveVelocity = Vector2.zero;

        float distanceToTarget = crab.GetDistanceToTarget();
        if (distanceToTarget < crab.distanceToHide && !crab.isInvisibleRecharging && crab.timer > crab.invisibleRechargeTime)
        {
            crab.ChangeState("Hide");
            return;
        }

        if (distanceToTarget < crab.distanceToShoot)
        {
            if (directionTimer < stopRotatingTime)
            {
                Vector2 dir = crab.GetDirectionToPosition(crab.target.gameObject.transform.position);
                crab.RotateToDirection(dir);
            }

            crab.ShootWeapon(crab.weapon, null, crab.firePoint, 7);
        }

        if (crabRifle.fireTimer == 0f)
            directionTimer = 0f;
    }
    
    public override void ExitState(Dictionary<string, object> args = null)
    {
        
    }
}
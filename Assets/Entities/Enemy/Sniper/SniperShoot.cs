using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class SniperShoot : State
{
    readonly Sniper sniper;

    public SniperShoot(Entity new_entity) : base(new_entity)
    {
        sniper = (Sniper) new_entity;
    }

    public override void EnterState(Dictionary<string, object> args = null)
    {

    }

    public override void UpdateState()
    {
        sniper.timer += Time.deltaTime; // hide recharge

        if (sniper.timer > sniper.rechargeTime)
        {
            sniper.isRecharging = false;
        }

        if (sniper.target == null)
        {
            Debug.LogError("Target not found");
            return;
        }

        float distanceToTarget = sniper.GetDistanceToTarget();

        /*
        if (distanceToTarget < sniper.distanceToHide)
        {
            sniper.ChangeState("Hide");
            return;
        }
        */

        if (distanceToTarget < sniper.distanceToShoot)
        {
            Vector2 dir = sniper.GetDirectionToPosition(sniper.target.gameObject.transform.position);
            sniper.RotateToDirection(dir);
            sniper.Visible();
            sniper.ShootWeapon(sniper.weapon, null, sniper.firePoint, 7);
        }

        if (sniper.GetDistanceToTarget() < 10f && !sniper.isRecharging && sniper.timer > sniper.rechargeTime)
        {
            sniper.ChangeState("Hide");
        }

    }
    
    public override void ExitState(Dictionary<string, object> args = null)
    {
        
    }
}
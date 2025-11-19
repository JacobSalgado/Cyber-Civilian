using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CyberBossMissileAttack: State
{
    readonly CyberBoss cyberBoss;
    private readonly Weapon missileWeapon;

    public CyberBossMissileAttack(Entity new_entity) : base(new_entity)
    {
        cyberBoss = (CyberBoss) new_entity;
        missileWeapon = cyberBoss.weapon.GetComponent<Weapon>();
    }

    public override void EnterState(Dictionary<string, object> args = null)
    {
        base.EnterState(args);
        Debug.Log("In Missile state");
    }

    public override void UpdateState()
    {
        // logic similar to sniper/homingshoot script
        if (cyberBoss.target == null)
        {
            Debug.Log("target not found");
            return;
        }

        float distanceToTarget = cyberBoss.GetDistanceToTarget();

        if (distanceToTarget < cyberBoss.distanceToShoot)
        {
            Vector2 dir = cyberBoss.GetDirectionToPosition(cyberBoss.target.gameObject.transform.position);
            cyberBoss.RotateToDirection(dir);

            cyberBoss.ShootWeapon(cyberBoss.weapon, null, cyberBoss.firePoint, 7);
        }

        cyberBoss.ShootWeapon(cyberBoss.weapon, null, cyberBoss.firePoint, 7);


        // --- Change States After Attack ---
        
        if (distanceToTarget < cyberBoss.distanceToMove)
        {
            cyberBoss.ChangeState("Travel");
            return;
        }
        else
        {
            cyberBoss.ChangeState("Idle");
            return;
        }
    }

    public override void ExitState(Dictionary<string, object> args = null)
    {
        base.ExitState(args);
    }
}

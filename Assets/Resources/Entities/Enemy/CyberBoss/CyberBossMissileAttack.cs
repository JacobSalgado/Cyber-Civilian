using System.Collections.Generic;
using UnityEngine;

public class CyberBossMissileAttack: State
{
    readonly CyberBoss cyberBoss;
    private readonly Weapon missileWeapon;

    private int shotCounter = 0;
    private float shotTimer = 0f;

    public CyberBossMissileAttack(Entity new_entity) : base(new_entity)
    {
        cyberBoss = (CyberBoss) new_entity;
        missileWeapon = cyberBoss.weapon.GetComponent<Weapon>();
    }

    public override void EnterState(Dictionary<string, object> args = null)
    {
        shotCounter = 0;
        shotTimer = 0f;
        Debug.Log("In Missile state");
    }

    public override void UpdateState()
    {
        if (cyberBoss.target == null)
        {
            Debug.Log("target not found");
            return;
        }

        cyberBoss.moveVelocity = Vector2.zero;
        float distanceToTarget = cyberBoss.GetDistanceToTarget();

        if (shotCounter >= cyberBoss.maxShots) // --- Change States After Attack ---
        {
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

        shotTimer += Time.deltaTime;
        if (distanceToTarget < cyberBoss.distanceToShoot)
        {
            Vector2 dir = cyberBoss.GetDirectionToPosition(cyberBoss.target.gameObject.transform.position);
            cyberBoss.RotateToDirection(dir);

            if (shotTimer > cyberBoss.shootTime)
            {
                cyberBoss.ShootWeapon(cyberBoss.weapon, null, cyberBoss.firePoint, 7);
                shotTimer = 0f;
                shotCounter++;
                Debug.Log(shotCounter);
            }
        }

    }

    public override void ExitState(Dictionary<string, object> args = null)
    {
        cyberBoss.SetCooldown(4.0f);
    }
}

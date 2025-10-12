using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.Interactions;

public class TrooperShoot : State
{
    readonly Trooper trooper;

    public TrooperShoot(Entity new_entity) : base(new_entity)
    {
        trooper = (Trooper)new_entity;
    }

    public override void EnterState(Dictionary<string, object> args = null)
    {
        trooper.rigidBody.linearVelocity = Vector2.zero;
    }

    public override void UpdateState()
    {
        float distance = trooper.GetDistanceToTarget();
        if (distance > -1f)
        {
            if (distance < trooper.distanceToShoot)
            {
                Vector2 dir = trooper.GetDirectionToPosition(trooper.target.gameObject.transform.position);
                trooper.RotateToDirection(dir);
                trooper.ShootWeapon(trooper.weapon, null, trooper.firePoint.position, trooper.firePoint.rotation, 7);
            }
            else if (distance < trooper.distanceToMove)
            {
                trooper.ChangeState("Idle");
                return;
            }
        }
    }
    
    public override void ExitState(Dictionary<string, object> args = null)
    {

    }
}
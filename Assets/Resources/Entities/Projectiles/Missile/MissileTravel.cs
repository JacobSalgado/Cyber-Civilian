using System.Collections.Generic;
using UnityEngine;

public class MissileTravel: State
{
    readonly Missile missile;

    public MissileTravel(Entity new_entity) : base(new_entity)
    {
        missile = (Missile)new_entity;
    }

    public override void EnterState(Dictionary<string, object> args = null)
    {
        
    }

    public override void UpdateState()
    {
        if (missile.projData.homing && missile.target != null)
        {
            Vector2 direction = missile.GetDirectionToPosition(missile.target.position);

            float rotateAmount = Vector3.Cross(direction, missile.transform.right).z;

            missile.rigidBody.angularVelocity = rotateAmount * missile.projData.rotateSpeed;

            Vector2 newDirection = Vector2.Lerp(-missile.transform.right, direction, missile.projData.trackingStrength * Time.deltaTime).normalized;
            missile.RotateToDirection(newDirection);
        }

        // move forward
        missile.rigidBody.linearVelocity = -missile.transform.right * missile.projData.moveSpeed;
    }
}
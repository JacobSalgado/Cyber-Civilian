using System;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileTravel : State
{
    readonly Projectile proj;
    Vector2 velocity;

    public ProjectileTravel(Entity new_entity) : base(new_entity)
    {
        proj = (Projectile) new_entity;
    }

    public override void EnterState(Dictionary<string, object> args = null)
    {
        proj.rigidBody.AddForce(proj.transform.right * -1f * proj.force, ForceMode2D.Impulse);
    }
}

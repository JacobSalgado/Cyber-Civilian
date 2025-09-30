using System.Collections.Generic;
using UnityEngine;

public class ProjectileIdle : State
{
    readonly Projectile proj;
    private float deltaCount = 0f;

    public ProjectileIdle(Entity new_entity) : base(new_entity)
    {
        proj = (Projectile)new_entity;
    }

    public override void UpdateState()
    {
        proj.ChangeState("Travel");
    }
}

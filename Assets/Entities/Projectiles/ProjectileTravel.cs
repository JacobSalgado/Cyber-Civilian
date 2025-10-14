using System.Collections.Generic;
using UnityEngine;

public class ProjectileTravel : State
{
    readonly Projectile proj;
    RaycastHit2D hit;
    Vector2 rayOrigin;
    Vector2 rayDirection;
    float raycastDistance;

    public ProjectileTravel(Entity new_entity) : base(new_entity)
    {
        proj = (Projectile)new_entity;
    }

    public override void EnterState(Dictionary<string, object> args = null)
    {
        proj.rigidBody.AddForce(proj.transform.right * -1f * proj.force, ForceMode2D.Impulse);
        rayOrigin = proj.transform.position;
        rayDirection = proj.transform.forward;
        raycastDistance = 10f;
    }

    public override void UpdateState()
    {
        //Debug.DrawRay(rayOrigin, rayDirection * raycastDistance, Color.red);
        //hit = Physics2D.Raycast(rayOrigin, rayDirection, raycastDistance);
    }
}

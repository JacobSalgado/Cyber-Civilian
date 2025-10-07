using System.Collections.Generic;
using UnityEngine;

public class BulletTravel : State
{
    readonly Bullet bullet;
    RaycastHit2D hit;
    Vector2 rayOrigin;
    Vector2 rayDirection;
    float raycastDistance;

    public BulletTravel(Entity new_entity) : base(new_entity)
    {
        bullet = (Bullet) new_entity;
    }

    public override void EnterState(Dictionary<string, object> args = null)
    {
        bullet.rigidBody.AddForce(bullet.transform.right * -1f * bullet.force, ForceMode2D.Impulse);
        rayOrigin = bullet.transform.position;
        rayDirection = bullet.transform.forward;
        raycastDistance = 10f;
    }

    public override void UpdateState()
    {
        //Debug.DrawRay(rayOrigin, rayDirection * raycastDistance, Color.red);
        //hit = Physics2D.Raycast(rayOrigin, rayDirection, raycastDistance);
    }
}

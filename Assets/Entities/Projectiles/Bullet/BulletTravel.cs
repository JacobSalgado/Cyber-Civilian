using System.Collections.Generic;
using UnityEngine;

public class BulletTravel : State
{
    readonly Bullet bullet;

    public BulletTravel(Entity new_entity) : base(new_entity)
    {
        bullet = (Bullet) new_entity;
    }

    public override void EnterState(Dictionary<string, object> args = null)
    {
        bullet.rigidBody.AddForce(-1f * bullet.force * bullet.transform.right, ForceMode2D.Impulse);
    }

    public override void UpdateState()
    {
        
    }
}

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
        
    }

    public override void UpdateState()
    {
        bullet.moveVelocity = bullet.transform.right * -bullet.projData.moveSpeed;
    }
}

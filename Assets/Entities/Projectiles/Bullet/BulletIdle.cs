using UnityEngine;

public class BulletIdle : State
{
    readonly Bullet bullet;
    //private float deltaCount = 0f;

    public BulletIdle(Entity new_entity) : base(new_entity)
    {
        bullet = (Bullet) new_entity;
    }

    public override void UpdateState()
    {
        bullet.ChangeState("Travel");
    }
}

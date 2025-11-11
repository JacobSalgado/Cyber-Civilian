using System.Collections.Generic;
using UnityEngine;

public class TrapShotTravel : State
{
    readonly TrapShot trapShot;
    private float lifeTime;

    public TrapShotTravel(Entity new_entity) : base(new_entity)
    {
        trapShot = (TrapShot) new_entity;
    }

    public override void EnterState(Dictionary<string, object> args = null)
    {
        
    }

    public override void UpdateState()
    {
        lifeTime -= Time.deltaTime;

        if (lifeTime <= 0f)
        {
            trapShot.EntityDie();
            return;
        }
        trapShot.rigidBody.linearVelocity = trapShot.transform.right * -trapShot.projData.moveSpeed;
    }
}

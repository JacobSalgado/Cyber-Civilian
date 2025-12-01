using System.Collections.Generic;
using UnityEngine;

public class TrapShotTravel : State
{
    readonly TrapShot trapShot;
    private float lifeTime;

    public TrapShotTravel(Entity new_entity) : base(new_entity)
    {
        trapShot = (TrapShot)new_entity;
        lifeTime = trapShot.projData.lifeTime;
    }

    public override void EnterState(Dictionary<string, object> args = null)
    {
        
    }

    public override void UpdateState()
    {
        lifeTime -= Time.deltaTime;

        if (lifeTime <= 0f)
        {
            trapShot.DeployField();
            trapShot.EntityDie();
            return;
        }
        trapShot.moveVelocity = trapShot.transform.right * -trapShot.projData.moveSpeed;
    }
}

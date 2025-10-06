using System.Collections.Generic;
using UnityEngine;

public class TrooperShoot : State
{
    readonly Trooper trooper;

    public TrooperShoot(Entity new_entity) : base(new_entity)
    {
        trooper = (Trooper)new_entity;
    }

    public override void EnterState(Dictionary<string, object> args = null)
    {

    }

    public override void UpdateState()
    {
        float distance = trooper.GetDistanceToTarget();
        if (distance > -1f)
        {
            if (distance < trooper.distanceToShoot)
            {
                // TODO: fire weapon here
                Debug.Log("Trooper fired shoot");
            }
            else if (distance < trooper.distanceToMove)
            {
                trooper.ChangeState("Idle");
                return;
            }
        }
    }
    
    public override void ExitState(Dictionary<string, object> args = null)
    {

    }
}
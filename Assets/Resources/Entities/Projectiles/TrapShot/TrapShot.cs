using UnityEngine;

public class TrapShot : Projectile
{
    public GameObject trapField;

    public override void InitializeStates()
    {
        AddState("Travel", new TrapShotTravel(this));

        ChangeState("Travel");
    }

    public override void Start()
    {
        base.Start();
        InitializeStates();
    }

    public void DeployField()
    {
        Instantiate(trapField, transform.position, Quaternion.identity, LevelManager.current_level.EntityList.transform);
        //Debug.Log("Trap Field Deployed");
    }

    public override void CollisionHit()
    {
        DeployField();
        base.CollisionHit();
    }
}
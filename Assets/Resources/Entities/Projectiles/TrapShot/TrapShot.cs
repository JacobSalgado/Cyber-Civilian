using UnityEngine;
public class TrapShot : Projectile
{
    public GameObject trapField;
    public override void InitializeStates()
    {
        AddState("Idle", new TrapShotIdle(this));
        AddState("Travel", new TrapShotTravel(this));

        ChangeState("Travel");
    }

    public override void Start()
    {
        base.Start();
        InitializeStates();
    }

    public override void EntityDie()
    {
        Instantiate(trapField, transform.position, Quaternion.identity, LevelManager.current_level.EntityList.transform);
        base.EntityDie();
    }
}
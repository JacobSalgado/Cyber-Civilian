using UnityEngine;

public class HammerStrike : Projectile
{
    public override void InitializeStates()
    {
        AddState("Base", new HammerBase(this));

        ChangeState("Base");
    }
    public override void Start()
    {
        base.Start();
        InitializeStates();
        Destroy(gameObject, projData.lifeTime);
    }
}
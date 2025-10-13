using System;
using UnityEngine;

public class Sniper : Enemy
{
    [Header("==Sniper Properties==")]
    public SpriteRenderer spriteRenderer;
    public float distanceToHide;

    // Non-Serialized vars
    [NonSerialized] public float distanceToShoot;

    public override void InitializeStates()
    {
        AddState("Idle", new SniperIdle(this));
        AddState("Hide", new SniperHide(this));
        AddState("Shoot", new SniperShoot(this));

        ChangeState("Idle");
    }

    public override void Start()
    {
        base.Start();
        InitializeStates();

        distanceToShoot = weapon.GetComponent<Weapon>().projData.railshotLength;
    }
}

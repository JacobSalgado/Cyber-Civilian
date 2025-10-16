using System;
using UnityEngine;

public class Sniper : Enemy
{
    [Header("==Sniper Properties==")]
    public SpriteRenderer spriteRenderer;
    public float distanceToHide;
    public Canvas healthCanvas;
    public CapsuleCollider2D sniperCollider;

    [Header("==Hide Properties==")]
    [NonSerialized] public bool isRecharging;
    [NonSerialized] public float rechargeTime = 5f;
    public float timer;

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

        sniperCollider = GetComponent<CapsuleCollider2D>();

        // hide properties
        timer = 0;
        isRecharging = false;
    }

    public void Invisible()
    {
        spriteRenderer.enabled = false;
        healthCanvas.enabled = false;
        sniperCollider.enabled = false;
    }

    public void Visible()
    {
        spriteRenderer.enabled = true;
        healthCanvas.enabled = true;
        sniperCollider.enabled = true;
    }
}

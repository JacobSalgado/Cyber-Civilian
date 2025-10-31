using System;
using UnityEngine;

public class Sniper : Enemy
{
    [Header("==Sniper GameObjects==")]
    public SpriteRenderer spriteRenderer;
    public CapsuleCollider2D sniperCollider;

    [Header("==Hide Properties==")]
    public float[] invisibleTimeRange = {0f, 1f};
    public float invisibleRechargeTime = 5f;
    
    // Non-Serialized vars
    [NonSerialized] public float distanceToHide;
    [NonSerialized] public float distanceToShoot;
    [NonSerialized] public bool isInvisibleRecharging = false;
    [NonSerialized] public bool isInvisible = false;
    [NonSerialized] public float timer = 0;
    private Canvas healthCanvas;

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

        healthCanvas = canvas.GetComponent<Canvas>();

        // NOTE: distanceToShoot for sniper is determined by given railshot length
        distanceToShoot = weapon.GetComponent<Weapon>().projData.railshotLength;
        distanceToHide = distanceToShoot * 0.5f;
    }

    public override void FixedUpdate()
    {
        base.FixedUpdate();

        timer += Time.deltaTime;
        
        isInvisibleRecharging = !isInvisible && timer <= invisibleRechargeTime;
        //Debug.Log(current_state);
    }

    public void Invisible()
    {
        isInvisible = true;
        spriteRenderer.enabled = false;
        healthCanvas.enabled = false;
        //sniperCollider.enabled = false;
        audioManager.PlayAudioSource("InvisibleStart");
    }

    public void Visible()
    {
        isInvisible = false;
        spriteRenderer.enabled = true;
        healthCanvas.enabled = true;
        //sniperCollider.enabled = true;
        audioManager.PlayAudioSource("InvisibleEnd");
    }
}

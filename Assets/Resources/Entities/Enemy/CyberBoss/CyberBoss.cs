using UnityEngine;
using System;

public class CyberBoss : Enemy
{
    [Header("==CyberBoss GameObjects==")]
    public SpriteRenderer spriteRenderer;
    public CyberBossStomp stomp;
    public CyberBossPunch punch;

    [Header("==Behavior Distances==")]
    public float distanceToShoot = 20f; // missile attack
    public float distanceToMove = 30f; // travel
    public float distanceToHit = 3f; // exploding punch attack
    public float distanceToStomp = 12f; // stomp attack

    [Header("==Vision Properites==")]
    public float seePlayerTimer = 0f;
    public float timeToSeePlayer = 1.0f;

    [Header("==Missile Attack Properites==")]
    public int maxShots = 5;
    public float shootTime = 0.35f;

    [Header("===Rage Mode Properties=")]
    public float healthThresholdForRage = 0.3f; // 30% of max health
    public float rageDamageBoost = 1.3f;
    public float rageSpeedBoost = 1.2f;

    [NonSerialized] public bool inRageMode = false;
    [NonSerialized] public float cooldownTime = 0f;
    [NonSerialized] public float cooldownEnd = 0f;

    [NonSerialized] public bool inCooldown = true;

    [Header("==Shield Properties==")]
    public CyberBossShield shield;
    [NonSerialized] public bool isShielding = true; // initially set to active when spawned
    public bool HasSeenPlayerLongEnough => seePlayerTimer >= timeToSeePlayer;

    public override void InitializeStates()
    {
        AddState("Idle", new CyberBossIdle(this));
        AddState("Travel", new CyberBossTravel(this));
        AddState("Missile", new CyberBossMissileAttack(this));
        AddState("Stomp", new CyberBossStompAttack(this));
        AddState("Punch", new CyberBossPunchAttack(this));

        ChangeState("Idle");
    }

    public override void Start()
    {
        base.Start();
        InitializeStates();
        type = EnemyTypes.CYBERBOSS;
    }

    public override void FixedUpdate()
    {
        if (!inRageMode && entityData.currentHealth < entityData.maxHealth * healthThresholdForRage)
            EnableRageMode();

        shield.Shield(isShielding); // activates shield
        invincibility = isShielding;
        //Debug.Log("Shield Health: " + shield.currentShieldHealth.ToString());


        base.FixedUpdate();

        if (inCooldown)
        {
            cooldownTime += Time.deltaTime;
            if (cooldownTime > cooldownEnd)
            {
                inCooldown = false;
                cooldownTime = 0f;
                cooldownEnd = 0f;
            }
        }
    }

    public bool CanSeePlayer() // Function for cyberboss to lock onto player
    { 
        Vector2 dir = (target.position - transform.position).normalized;
        float dist = GetDistanceToTarget();

        // raycast to check line of sight
        RaycastHit2D hit = Physics2D.Raycast(transform.position, dir, dist, 6);

        return hit && hit.transform.gameObject.layer == LayerMask.NameToLayer("Player");
    }

    public void UpdateVision()
    {
        if (CanSeePlayer())
        {
            seePlayerTimer += Time.deltaTime;
        }
        else
        {
            seePlayerTimer = 0f;
        }
    }
    

    public void SetCooldown(float newCooldown)
    {
        inCooldown = true;
        cooldownTime = 0f;
        cooldownEnd = newCooldown;
    }

    public void EnableRageMode()
    {
        inRageMode = true;
        spriteRenderer.color = Color.red; // changes color to red to indicate rage mode

        // apply multipliers
        entityData.moveSpeed *= rageSpeedBoost;
        stomp.damage = Mathf.CeilToInt(stomp.damage * rageDamageBoost);
        stomp.moveSpeed *= rageSpeedBoost;
        punch.damage = Mathf.CeilToInt(punch.damage * rageDamageBoost);
        // NOTE: missile damage buffed in Weapon script
    }

    // ============================
    // SHIELD FUNCTIONS
    // ============================
    public void EquipShield()
    {
        isShielding = true;
        // TODO: play SFX
    }

    public void UnequipShield()
    {
        isShielding = false;
        // TODO: play SFX
    }
}

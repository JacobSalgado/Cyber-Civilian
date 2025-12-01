using UnityEngine;
using System;

public class CyberBoss : Enemy
{
    [Header("==Cyber Boss Properties==")]
    public SpriteRenderer spriteRenderer;
    public Stomp stomp;
    public Punch punch;

    public GameObject[] weaponsList;

    public float distanceToShoot = 20f; // missile attack
    public float distanceToMove = 30f; // travel
    public float distanceToHit = 3f; // exploding punch attack
    public float distanceToStomp = 12f; // stomp attack

    //------- Locking onto player -----------
    public float seePlayerTimer = 0f;
    public float timeToSeePlayer = 1.0f;

    [NonSerialized] public float cooldownTime = 0f;
    [NonSerialized] public float cooldownEnd = 0f;

    [NonSerialized] public bool inCooldown = true;
    public bool HasSeenPlayerLongEnough => seePlayerTimer >= timeToSeePlayer;

    // ---------- Rage Mode Variables -----------
    public bool inRageMode = false;
    public float healthThresholdForRage = 0.3f; // 30% of max health


    public override void InitializeStates()
    {
        AddState("Idle", new CyberBossIdle(this));
        AddState("Travel", new CyberBossTravel(this));
        AddState("Missile", new CyberBossMissileAttack(this));
        AddState("Stomp", new CyberBossStompAttack(this));
        AddState("Punch", new CyberBossPunchAttack(this));
        //AddState("Rage", new CyberBossRage(this)); // no rage state, just rage function with buffs to other states

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

        //entityData.moveSpeed *= 1.2f; // need to call with bool flag to avoid stacking speed increases

        entityData.moveSpeed = Mathf.Min(entityData.moveSpeed * 1.2f, entityData.maxMoveSpeed);
    }
}

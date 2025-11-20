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

    public float punchSpeed = 3.0f;

    //------- Locking onto player -----------
    public float seePlayerTimer = 0f;
    public float timeToSeePlayer = 1.0f;

    [NonSerialized] public float cooldownTime = 0f;
    [NonSerialized] public float cooldownEnd = 0f;

    [NonSerialized] public bool inCooldown = true;
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
}

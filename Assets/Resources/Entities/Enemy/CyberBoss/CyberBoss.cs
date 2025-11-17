using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class CyberBoss : Enemy
{
    [Header("==Cyber Boss Properties==")]
    public SpriteRenderer spriteRenderer;
    public Stomp stomp;

    public float distanceToShoot = 10f; // missile attack
    public float distanceToMove = 25f; // travel
    public float distanceToHit = 2f; // exploding punch attack
    public float distanceToStomp = 15f; // stomp attack

    //------- Locking onto player -----------
    public float seePlayerTimer = 0f;
    public float timeToSeePlayer = 1.0f;

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
    
}

using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class CyberBoss : Enemy
{
    [Header("==Cyber Boss Properties==")]
    public SpriteRenderer spriteRenderer;
    public Stomp stomp;

    public float distanceToShoot; // missile attack
    public float distanceToMove; // travel
    public float distanceToHit; // exploding punch attack
    public float distanceToStomp; // stomp attack

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

    
}

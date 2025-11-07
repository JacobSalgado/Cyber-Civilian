using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public abstract class CyberBoss : Enemy
{
    [Header("==Cyber Boss Properties==")]
    public SpriteRenderer spriteRenderer;

    [Header("==Missile Properties==")]
    [SerializeField] private GameObject missilePrefab;
    [SerializeField] private Transform missileSpawnPoint;
    [SerializeField] private float missileSpeed = 10f;
    [SerializeField] private float missileCooldown = 5f;
    [SerializeField] private int missileVolleyCount = 3;
    [SerializeField] private float missileInterval = 0.5f;

    [Header("==Stomp Properties==")]
    [SerializeField] private float stompRange = 5f;
    [SerializeField] private float stompDamage = 20f;
    [SerializeField] private float stompCooldown = 8f;
    [SerializeField] private float stompKnockback = 15f;
    [SerializeField] private LayerMask playerLayer; // stomp affects only player

    [Header("==Movement==")]
    [SerializeField] private float moveSpeed = 3f;
    [SerializeField] private float stopDistance = 5f;
    [SerializeField] private float attackRange = 15f;

    [Header("==AI Behavior==")]
    [SerializeField] private float decisionInterval = 1f;
    [SerializeField] private float missileAttackChance = 0.6f; // 60% chance to choose missile attack

    private float nextMissileTime;
    private float nextStompTime;
    private float nextDecisionTime;
    private Coroutine currentAttackCoroutine;

    //public Transform playerTarget;

    // public properties for states
    public float MoveSpeed => moveSpeed;
    public float StopDistance => stopDistance;
    public float AttackRange => attackRange;
    public float StompRange => stompRange;
    public Transform Target => target;

    public override void InitializeStates()
    {
        AddState("Idle", new CyberBossIdle(this));
        AddState("Travel", new CyberBossTravel(this));
        AddState("MissileAttack", new CyberBossMissileAttack(this));
        AddState("StompAttack", new CyberBossStompAttack(this));

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

        // Decision making based on interval
        if (Time.time >= nextDecisionTime && current_state?.GetType().Name == "CyberBossIdle")
        {
            nextDecisionTime = Time.time + decisionInterval;
            MakeDecision();
        }
    }

    private void MakeDecision()
    {
        if (target == null) return;

        float distanceToTarget = Vector2.Distance(transform.position, target.position);

        // If far away from target, travel closer
        if (distanceToTarget > attackRange)
        {
            ChangeState("Travel");
        }

        // if in stomp range, stomp attack
        else if (distanceToTarget <= stompRange && CanStompAttack())
        {
            ChangeState("StompAttack");
        }
        // if in attack range, decide between missile or move closer
        else if (distanceToTarget <= attackRange && CanMissileAttack())
        {
            float rand = Random.Range(0f, 1f);
            if (rand <= missileAttackChance)
            {
                ChangeState("MissileAttack");
            }
            else
            {
                ChangeState("Travel");
            }
        }
        else if (distanceToTarget > stopDistance)
        {
            ChangeState("Travel");
        }
    }

    public bool CanMissileAttack()
    {
        return Time.time >= nextMissileTime;
    }

    public bool CanStompAttack()
    {
        return Time.time >= nextStompTime;
    }

    public void StartMissileVolley()
    {
        if (currentAttackCoroutine != null)
            StopCoroutine(currentAttackCoroutine);
        currentAttackCoroutine = StartCoroutine(PerformMissileVolley());
    }

    public void StartStompAttack()
    {
        if (currentAttackCoroutine != null)
            StopCoroutine(currentAttackCoroutine);
        PerformStomp();
    }

    public IEnumerator PerformMissileVolley()
    {
        nextMissileTime = Time.time + missileCooldown;
        rigidBody.linearVelocity = Vector2.zero;

        for (int i = 0; i < missileVolleyCount; i++)
        {
            LaunchMissile();
            yield return new WaitForSeconds(missileInterval);
        }

        yield return new WaitForSeconds(0.5f);
        ChangeState("Idle");
    }

    private void LaunchMissile()
    {
        if (missilePrefab == null || missileSpawnPoint == null || target == null) return;

        GameObject missile = Instantiate(missilePrefab, missileSpawnPoint.position, Quaternion.identity);

        Vector2 direction = (target.position - missileSpawnPoint.position).normalized;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        missile.transform.rotation = Quaternion.Euler(0, 0, angle);

        Rigidbody2D missileRb = missile.GetComponent<Rigidbody2D>();
        if (missileRb != null)
        {
            missileRb.linearVelocity = direction * missileSpeed;
        }
    }

    public void PerformStomp()
    {
        nextStompTime = Time.time + stompCooldown;
        rigidBody.linearVelocity = Vector2.zero;

        // Detect player in range
        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, stompRange, playerLayer);

        foreach (Collider2D hit in hits)
        {
            // damage player 
            if (hit.TryGetComponent<Player>(out var player))
            {
                player.TakeDamage((int)stompDamage);
                // Apply knockback
                Vector2 knockbackDir = (player.transform.position - transform.position).normalized;
                player.rigidBody.AddForce(knockbackDir * stompKnockback, ForceMode2D.Impulse);
            }
        }

        // for audio effects
        if (audioManager != null)
        {
            audioManager.PlayAudioSource("Stomp");
        }

        // visual feedback (particles, screen shake)
        StartCoroutine(StompVisualFeedback());
    }

    private IEnumerator StompVisualFeedback()
    {
        // flash effect
        Vector3 originalScale = transform.localScale;
        transform.localScale = originalScale * 1.1f;
        yield return new WaitForSeconds(0.1f);
        transform.localScale = originalScale;

        yield return new WaitForSeconds(0.5f);
        ChangeState("Idle");
    }

    public void MoveTowardsTarget()
    {
        if (target == null) return;

        Vector2 direction = (target.position - transform.position).normalized;
        rigidBody.linearVelocity = direction * moveSpeed;
    }

    public void StopMovement()
    { 
        rigidBody.linearVelocity = Vector2.zero;
    }

    public float GetDistanceToTarget()
    {
        if (target == null) return Mathf.Infinity;
        return Vector2.Distance(transform.position, target.position);
    }

    public override void GotDamaged()
    {
        // visual feedback when damaged
        if (spriteRenderer != null)
        {
            StartCoroutine(DamageFlash());
        }

        // hurt sound
        if (audioManager != null)
        { 
            audioManager.PlayAudioSource("CyberBossHurt");
        }
    }

    private IEnumerator DamageFlash()
    {
        Color originalColor = spriteRenderer.color;
        spriteRenderer.color = Color.red;
        yield return new WaitForSeconds(0.1f);
        spriteRenderer.color = originalColor;
        isDamaged = false;
    }

    public override void EntityDie()
    {
        // play death sound
        if (audioManager != null)
        { 
            audioManager.PlayAudioSource("CyberBossDeath");
        }

        base.EntityDie();
    }
}

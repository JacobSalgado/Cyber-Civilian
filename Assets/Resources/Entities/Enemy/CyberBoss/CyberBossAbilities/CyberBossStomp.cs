using System;
using UnityEngine;

public class CyberBossStomp : MonoBehaviour
{
    private const float TTL = 1.2f;
    private float timer;

    [Header("==Stomp GameObjects==")]
    [SerializeField] private CyberBoss cyberBoss;
    public CircleCollider2D aoeCollider;
    public Rigidbody2D rb;
    public SpriteRenderer spriteRenderer;

    [Header("==Stomp Properties==")]
    [SerializeField] private float pushForce = 15f;
    [SerializeField] private float pushedTime = 2f;
    public float moveSpeed = 8f;
    public int damage = 200;

    [Header("==Stomp Size Behavior==")]
    [SerializeField] private float maxScale = 3f;
    [SerializeField] private float scaleIncRate = 0.4f;

    [NonSerialized] public Vector2 directionToPlayer;

    public Vector3 startScale;
    private bool isActive;

    void Awake()
    {
        startScale = transform.localScale;
        ResetAOE();
    }

    void FixedUpdate()
    {
        if (!isActive) return;

        timer += Time.deltaTime;

        // Scale outward
        if (transform.localScale.x < maxScale)
        {
            float scaleDelta = scaleIncRate * Time.deltaTime;
            transform.localScale += Vector3.one * scaleDelta;
        }

        // Move forward
        rb.linearVelocity = directionToPlayer * moveSpeed;

        if (timer >= TTL)
        {
            ResetAOE();
        }
    }

    // ============================
    // ACTIVATE STOMP
    // ============================
    public void EmitPush(Vector2 spawnPosition, Vector2 moveDirection)
    {
        if (isActive) return;

        transform.position = spawnPosition;
        directionToPlayer = moveDirection.normalized;

        transform.localScale = startScale;
        timer = 0f;
        isActive = true;

        aoeCollider.enabled = true;
        spriteRenderer.enabled = true;

        // play particles
        cyberBoss.stompEffect.transform.position = spawnPosition;
        cyberBoss.stompEffect.Play();
    }

    // ============================
    // RESET STOMP
    // ============================
    private void ResetAOE()
    {
        isActive = false;

        aoeCollider.enabled = false;
        spriteRenderer.enabled = false;

        rb.linearVelocity = Vector2.zero;
        transform.localScale = startScale;
    }

    // ============================
    // COLLISION
    // ============================
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!isActive) return;

        if (collision.TryGetComponent<Player>(out var player))
        {
            Vector2 vectorToCheck =
                (transform.position - player.transform.position).normalized;

            if (player.isShielding &&
                player.shieldAbility.IsShieldBlocking(vectorToCheck))
                return;

            player.TakeDamage(damage);

            if (!player.pushed)
            {
                player.pushed = true;
                Vector2 pushDir =
                    player.GetDirectionToPosition(transform.position);

                player.pushedVelocity = -pushForce * pushDir;
                player.pushedTime = pushedTime;
            }
        }
    }
}

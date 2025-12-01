using System;
using UnityEngine;

public class CyberBossPunch : MonoBehaviour
{
    private float timer = 0f;

    [SerializeField] private Transform initialLocalPosition;
    public CircleCollider2D punchCollider;
    [SerializeField] private Rigidbody2D punchRigidbody;
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private float timeToLive = 0.2f;
    [SerializeField] private int damage = 80;
    public float punchSpeed = 3.0f;

    [NonSerialized] public Vector2 directionToPlayer = Vector2.zero;

    void Start()
    {
        punchCollider.enabled = false;
        spriteRenderer.enabled = false;
    }

    void Update()
    {
        gameObject.transform.position = initialLocalPosition.position;
        if (punchCollider.enabled)
        {
            timer += Time.deltaTime;

            //punchRigidbody.linearVelocity = punchSpeed * directionToPlayer;
            if (timer >= timeToLive)
            {
                timer = 0f;
                punchCollider.enabled = false;
                spriteRenderer.enabled = false;
            }
        }
        //else gameObject.transform.localPosition = Vector2.zero;
    }

    public void EmitPunch()
    {
        if (!punchCollider.enabled)
        {
            punchCollider.enabled = true;
            spriteRenderer.enabled = true;
            timer = 0f;
        } 
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        //print(collision.gameObject.name);
        if (collision.gameObject.TryGetComponent<Player>(out var player))
        {
            player.TakeDamage(damage);
            //ApplyKnockback(player);
        }
    }

    private void ApplyKnockback(Player player)
    { 
        Rigidbody2D playerRb = player.GetComponent<Rigidbody2D>();

        if (playerRb != null)
        {
            // direction from punch to player
            Vector2 knockbackDirection = (player.transform.position - transform.position).normalized;

            // knockback force
            float knockbackForce = 1000f;

            // force applied here
            //playerRb.AddForce(knockbackDirection * knockbackForce, ForceMode2D.Impulse);
            playerRb.linearVelocity = knockbackDirection * knockbackForce;

            Debug.Log("knockback forced applied");

        }
    }
}

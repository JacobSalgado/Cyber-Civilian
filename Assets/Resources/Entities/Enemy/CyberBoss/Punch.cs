using System;
using UnityEngine;

public class Punch : MonoBehaviour
{
    private const float TTL = 0.2f;
    private float timer = 0f;
    [SerializeField] private Transform initialLocalPosition;

    public float punchSpeed = 3.0f;
    public CircleCollider2D punchCollider;
    public Rigidbody2D punchRigidbody;
    public SpriteRenderer spriteRenderer;

    [NonSerialized] public Vector2 directionToPlayer = Vector2.zero;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        punchCollider.enabled = false;
        spriteRenderer.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        gameObject.transform.position = initialLocalPosition.position;
        if (punchCollider.enabled)
        {
            timer += Time.deltaTime;

            //punchRigidbody.linearVelocity = punchSpeed * directionToPlayer;
            if (timer >= TTL)
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
            player.TakeDamage(50);
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

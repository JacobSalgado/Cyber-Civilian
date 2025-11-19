using JetBrains.Annotations;
using System.Runtime.CompilerServices;
using UnityEngine;

public class Punch : MonoBehaviour
{
    private const float TTL = 1f;
    private float timer = 0f;

    public CircleCollider2D punchCollider;
    public Rigidbody2D punchRigidbody;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        punchCollider.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (punchCollider.enabled)
        {
            timer += Time.deltaTime;

            //punchRigidbody.linearVelocity = move

            if (timer >= TTL)
            {
                timer = 0f;
                punchCollider.enabled = false;
            }
        }
    }

    public void EmitPunch(Vector2 punchDirection)
    {
        if (!punchCollider.enabled)
        {
            float punchReach = 1.5f;
            transform.localPosition = punchDirection.normalized * punchReach;

            punchCollider.enabled = true;
            timer = 0f;
        } 
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        print(collision.gameObject.name);
        if (collision.gameObject.TryGetComponent<Player>(out var player))
        {
            player.TakeDamage(50);
            ApplyKnockback(player);
            punchCollider.enabled = false;
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

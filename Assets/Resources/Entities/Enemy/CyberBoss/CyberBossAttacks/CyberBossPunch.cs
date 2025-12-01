using System;
using UnityEngine;

public class CyberBossPunch : MonoBehaviour
{
    private float timer = 0f;

    [Header("==Necessary GameObjects==")]
    [SerializeField] private CyberBoss _cyberBoss;
    [SerializeField] private Transform initialLocalPosition;
    public CircleCollider2D punchCollider;
    [SerializeField] private Rigidbody2D punchRigidbody;
    [SerializeField] private SpriteRenderer spriteRenderer;

    [Header("==Punch Properties==")]
    [SerializeField] private float timeToLive = 0.2f;
    [SerializeField] private float force = 5f;
    [SerializeField] private float pushedTime = 3f;
    public int damage = 80;
    public float punchSpeed = 3.0f;

    [NonSerialized] public Vector2 directionToPlayer = Vector2.zero;

    void Start()
    {
        punchCollider.enabled = false;
        spriteRenderer.enabled = false;
    }

    void FixedUpdate()
    {
        gameObject.transform.position = initialLocalPosition.position;
        if (punchCollider.enabled)
        {
            timer += Time.deltaTime;

            if (timer >= timeToLive)
            {
                timer = 0f;
                punchCollider.enabled = false;
                spriteRenderer.enabled = false;
            }
        }
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
        if (collision.gameObject.TryGetComponent<Player>(out var player)) {
            Vector2 vectorToCheck = (gameObject.transform.position - player.gameObject.transform.position).normalized;
            if (player.isShielding && player.shieldAbility.IsShieldBlocking(vectorToCheck)) 
                return;

            player.TakeDamage(damage);

            // apply knockback if in rage mode
            if (_cyberBoss.inRageMode && !player.pushed){
                player.pushed = true;

                Vector2 pushDirection = player.GetDirectionToPosition(gameObject.transform.position);
                player.pushedVelocity = -force * pushDirection;

                player.pushedTime = pushedTime;
            }
        }
    }
}

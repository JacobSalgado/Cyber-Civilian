using System;
using UnityEngine;

public class Stomp : MonoBehaviour
{
    private const float TTL = 1.2f;
    private float timer = 0f;

    [SerializeField] public float moveSpeed = 8f; // made this variable public for easier access in CyberBossStompAttack

    [SerializeField] public float maxShockwaveRadius = 20f; // made this variable public for easier access in CyberBossStompAttack
    [SerializeField] private float pushForce = 15f;

    [NonSerialized] public Vector3 startScale = Vector3.zero;
    [SerializeField] private Vector3 maxScale = Vector3.zero;
    [SerializeField] public float scaleIncRate = 0.4f; // changed to public for easier access in CyberBossStompAttack

    public CircleCollider2D aoeCollider;
    public Rigidbody2D rigidBody;
    public SpriteRenderer spriteRenderer;

    // hashset if we want to make it so the aoe affects other enemies as well
    // private HashSet<Collider2D> hitTargets = new HashSet<Collider2D>();

    [NonSerialized] public Vector2 directionToPlayer = Vector2.zero;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        aoeCollider.enabled = false;
        spriteRenderer.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (aoeCollider.enabled)
        {
            timer += Time.deltaTime;
            if (gameObject.transform.localScale.x < maxScale.x) // NOTE: scale should be uniform
            {
                Vector3 newScale = gameObject.transform.localScale;
                newScale.x += scaleIncRate * Time.deltaTime;
                newScale.y += scaleIncRate * Time.deltaTime;
                newScale.z += scaleIncRate * Time.deltaTime;
                gameObject.transform.localScale = newScale;
            }
                
            rigidBody.linearVelocity = moveSpeed * directionToPlayer;
            
            if (timer > TTL)
            {
                timer = 0f;
                aoeCollider.enabled = false;
                spriteRenderer.enabled = false;
                rigidBody.linearVelocity = Vector2.zero;
            }
        }
        else gameObject.transform.localPosition = Vector2.zero;
    }

    public void EmitPush()
    {
        if (!aoeCollider.enabled) {
            aoeCollider.enabled = true;
            spriteRenderer.enabled = true;
        }
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        CyberBoss cyberBoss = GetComponentInParent<CyberBoss>();

        // initialize push velocity
        print(collision.gameObject.name);
        if (collision.gameObject.TryGetComponent<Player>(out var player))
        {
            player.pushed = true;
            Vector2 pushDirection = player.GetDirectionToPosition(gameObject.transform.position);

            // rage mode check
            if (cyberBoss.inRageMode)
            {
                player.TakeDamage(250);
            }
            else
            {
                player.TakeDamage(200); // make player take damage if stomp aoe collides with them   
            }
            player.pushedVelocity = -pushForce * pushDirection;
        }
    }
}

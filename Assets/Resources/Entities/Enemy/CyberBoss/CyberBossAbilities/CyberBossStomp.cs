using System;
using UnityEngine;

public class CyberBossStomp : MonoBehaviour
{
    private const float TTL = 1.2f;
    private float timer = 0f;

    [Header("==Stomp GameObjects==")]
    [SerializeField] private CyberBoss _cyberBoss;
    public CircleCollider2D aoeCollider;
    public Rigidbody2D rigidBody;
    public SpriteRenderer spriteRenderer;

    [Header("==Stomp Properties==")]
    [SerializeField] private float pushForce = 15f;
    [SerializeField] private float pushedTime = 2f;
    public float moveSpeed = 8f;
    public int damage = 200;

    [Header("==Stomp Size Behavior==")]
    [SerializeField] private Vector3 maxScale = Vector3.zero;
    [SerializeField] private float scaleIncRate = 0.4f;

    [NonSerialized] public Vector3 startScale = Vector3.zero;
    [NonSerialized] public Vector2 directionToPlayer = Vector2.zero;

    // hashset if we want to make it so the aoe affects other enemies as well
    // private HashSet<Collider2D> hitTargets = new HashSet<Collider2D>();

    void Start()
    {
        aoeCollider.enabled = false;
        spriteRenderer.enabled = false;
    }

    void FixedUpdate()
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
            _cyberBoss.stompEffect.Play();
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

            if (!player.pushed) {
                player.pushed = true;

                Vector2 pushDirection = player.GetDirectionToPosition(gameObject.transform.position);
                player.pushedVelocity = -pushForce * pushDirection;

                player.pushedTime = pushedTime;
            }
        }
    }
}

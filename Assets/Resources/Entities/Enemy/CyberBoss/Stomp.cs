using System;
using UnityEngine;

public class Stomp : MonoBehaviour
{
    private const float TTL = 3.5f;
    private float timer = 0f;

    [SerializeField] float moveSpeed = 4f;
    
    [SerializeField] private float maxShockwaveRadius = 20f;
    [SerializeField] private float pushForce = 15f;

    [NonSerialized] public float startRadius = 0f;
    [SerializeField] private float maxRadius = 6f;
    [SerializeField] private float radiusIncRate = 0.4f;

    public CircleCollider2D aoeCollider;
    public Rigidbody2D rigidBody;

    //public SpriteRenderer spriteRenderer;

    // hashset if we want to make it so the aoe affects other enemies as well
    // private HashSet<Collider2D> hitTargets = new HashSet<Collider2D>();

    [NonSerialized] public Vector2 directionToPlayer = Vector2.zero;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        aoeCollider.enabled = false;
        //spriteRenderer.transform.localScale = Vector3.zero;
    }

    // Update is called once per frame
    void Update()
    {
        if (aoeCollider.enabled)
        {
            timer += Time.deltaTime;
            if (aoeCollider.radius < maxRadius)
            {
                aoeCollider.radius += Time.deltaTime * radiusIncRate;
            }

            float diameter = aoeCollider.radius * 2f;
            //spriteRenderer.transform.localScale = new Vector3(diameter, diameter, 1f);
                

            rigidBody.linearVelocity = moveSpeed * directionToPlayer;
            
            if (timer > TTL)
            {
                timer = 0f;
                aoeCollider.enabled = false;
            }
        }
        else gameObject.transform.localPosition = Vector2.zero;

        
    }

    public void EmitPush()
    {
        if (!aoeCollider.enabled) aoeCollider.enabled = true;

        /*if (!aoeCollider.enabled)
        {
            aoeCollider.enabled = true;
            spriteRenderer.enabled = true;
            spriteRenderer.transform.localScale = Vector3.zero;
            aoeCollider.radius = 0f;
        }*/
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        // initialize push velocity
        print(collision.gameObject.name);
        if (collision.gameObject.TryGetComponent<Player>(out var player))
        {
            //player.pushed = true;
            Vector2 pushDirection = player.GetDirectionToPosition(gameObject.transform.position);
            player.TakeDamage(200); // make player take damage if stomp aoe collides with them
            //player.pushedVelocity = -force * pushDirection;
        }
    }

    private void DrawAOEStomp()
    {
        float diameter = aoeCollider.radius * 2f;

        transform.localScale = new Vector3(diameter, diameter, 1f);
    }
}

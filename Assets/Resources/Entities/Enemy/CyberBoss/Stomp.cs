using UnityEngine;

public class Stomp : MonoBehaviour
{
    private const float TTL = 0.3f;
    private float timer = 0f;

    public CircleCollider2D aoeCollider;
    [SerializeField] float force = 1f;

    public Rigidbody2D rigidbody2D;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        aoeCollider.enabled = false;    

        rigidbody2D = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        gameObject.transform.localPosition = Vector2.zero;
        if (aoeCollider.enabled)
        {
            timer += Time.deltaTime;
            if (timer > TTL)
            {
                timer = 0f;
                aoeCollider.enabled = false;
                rigidbody2D.linearVelocity = new Vector2(20, 10);
            }
        }

        
    }

    public void EmitPush()
    {
        if (!aoeCollider.enabled) aoeCollider.enabled = true;
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
}

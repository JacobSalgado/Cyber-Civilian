using UnityEngine;

public class MantisPunch : MonoBehaviour
{
    private float timer = 0f;

    [Header("==Necessary GameObjects==")]
    [SerializeField] private Mantis mantis;
    public CircleCollider2D punchCollider;
    [SerializeField] private Rigidbody2D punchRigidbody;
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private Transform initialLocalPosition;

    [Header("==Punch Properties==")]
    [SerializeField] private float timeToLive = 0.2f;
    [SerializeField] private int damage = 80;

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
        if (collision.gameObject.TryGetComponent<Player>(out var player))
        {
            if (player.isShielding)
            {
                Vector2 toMantis = -mantis.GetDirectionToPosition(player.transform.position);

                if (player.shieldAbility.IsShieldBlocking(toMantis))
                    return;
            }
                
            player.TakeDamage(damage);
        }
    }
}

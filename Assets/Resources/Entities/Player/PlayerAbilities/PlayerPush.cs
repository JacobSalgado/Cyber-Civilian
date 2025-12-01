using UnityEngine;

public class PlayerPush : MonoBehaviour
{
    private const float TTL = 0.3f;
    private float timer = 0f;

    [SerializeField] private CircleCollider2D aoe;
    [SerializeField] private float force = 1f;
    [SerializeField] private float pushedTime = 3.0f;

    void Start()
    {
        aoe.enabled = false;
    }

    void Update()
    {
        gameObject.transform.localPosition = Vector2.zero;
        gameObject.transform.localRotation = Quaternion.identity;
        if (aoe.enabled)
        {
            timer += Time.deltaTime;
            if (timer > TTL)
            {
                timer = 0f;
                aoe.enabled = false;
            }
        }
    }

    public void EmitPush()
    {
        if (!aoe.enabled) aoe.enabled = true;
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        // initialize push velocity
        //print(collision.gameObject.name);
        if (collision.gameObject.TryGetComponent<Enemy>(out var enemy))
        {
            enemy.pushed = true;

            Vector2 pushDirection = enemy.GetDirectionToPosition(gameObject.transform.position);
            enemy.pushedVelocity = -force * pushDirection;
            
            enemy.pushedTime = pushedTime;
        }
    }
}

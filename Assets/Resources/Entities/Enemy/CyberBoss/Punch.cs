using JetBrains.Annotations;
using UnityEngine;

public class Punch : MonoBehaviour
{
    public CircleCollider2D punchCollider;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        punchCollider.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void EmitPunch()
    {
        if (!punchCollider.enabled) punchCollider.enabled = true;
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        print(collision.gameObject.name);
        if (collision.gameObject.TryGetComponent<Player>(out var player))
        {
            player.TakeDamage(200);
        }
    }
}

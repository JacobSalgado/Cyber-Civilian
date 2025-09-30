using UnityEngine;

public class Projectile : Entity
{
    public GameObject hitEffect;
    public float damage = 1f;
    public float force;
    public Vector2 mousePos;

    public override void InitializeStates()
    {
        AddState("Idle", new ProjectileIdle(this));
        AddState("Travel", new ProjectileTravel(this));

        ChangeState("Travel");
    }

    void Start()
    {
        InitializeStates();
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        GameObject effect = Instantiate(hitEffect, transform.position, Quaternion.identity);
        Destroy(effect, 0.1f);
        Destroy(gameObject);
    }
}

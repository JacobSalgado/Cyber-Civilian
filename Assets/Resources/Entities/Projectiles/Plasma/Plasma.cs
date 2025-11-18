using UnityEngine;
public class Plasma : Projectile
{
    public Collider2D damageFieldCollider;
    public SpriteRenderer damageFieldSpriteRenderer;
    public int damageFieldDamage = 30;
    public int damageFieldFireDamage = 1;
    public float damageFieldFireDuration = 2f;
    public override void InitializeStates()
    {
        AddState("Idle", new PlasmaIdle(this));
        AddState("Travel", new PlasmaTravel(this));

        ChangeState("Travel");
    }

    public override void Start()
    {
        base.Start();
        InitializeStates();
    }

    public override void OnTriggerEnter2D(Collider2D collision)
    {
        if (damageFieldCollider.IsTouching(collision))
        {
            if (collision.gameObject.TryGetComponent<Enemy>(out var enemy))
            {
                enemy.TakeDamage(damageFieldDamage);
                enemy.ApplyOnFireEffect(damageFieldFireDuration, damageFieldFireDamage);
            }
        }

        if (projectileCollider.IsTouching(collision))
        {
            base.OnTriggerEnter2D(collision);
        }
    }

    public override void CollisionHit()
    {
        damageFieldCollider.enabled = false;
        damageFieldSpriteRenderer.enabled = false;
        base.CollisionHit();
    }
}
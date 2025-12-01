using UnityEngine;
public class Plasma : Projectile
{
    [Header("==Damage Field GameObjects==")]
    public Collider2D damageFieldCollider;
    public SpriteRenderer damageFieldSpriteRenderer;

    [Header("==Damage Field Properties==")]
    public int damageFieldDamage = 30;
    public int damageFieldFireDamage = 10;
    public float damageFieldFireDuration = 1.2f;


    public override void InitializeStates()
    {
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
                enemy.ApplyFireEffect(damageFieldFireDuration, damageFieldFireDamage);
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
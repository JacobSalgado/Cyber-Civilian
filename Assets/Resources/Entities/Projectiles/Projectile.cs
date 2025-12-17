using System;
using UnityEngine;

public abstract class Projectile : Entity
{
    [Header("==Projectile GameObjects==")]
    public SpriteRenderer spriteRenderer;
    public Collider2D projectileCollider;

    [NonSerialized] public ProjectileData projData;
    [NonSerialized] public LayerMask attacking_layer = 0;
    private bool collisionHit = false;
    private float timer = 0f;

    [Header("==Player VFX==")]
    public ParticleSystem playerHUDFireEffect;

    public override void Start()
    {
        base.Start();
        timer = 0f;
        collisionHit = false;
    }

    public void Update()
    {
        if (collisionHit) timer += Time.deltaTime;

        if ((!audioManager.audioEffects.ContainsKey("Impact") && collisionHit) || 
             audioManager.audioEffects.ContainsKey("Impact") && timer > audioManager.audioEffects["Impact"].clip.length)
            EntityDie();
    }

    public virtual void OnTriggerEnter2D(Collider2D collision)
    {
        // Player Collision
        if (attacking_layer == 7 && collision.gameObject.TryGetComponent<Player>(out var player))
        {         
            player.TakeDamage(projData.damage);
            CheckForStatusEffect(player);

            // Destroy projectile if necessary (blocked or hit)
            HitEffect(transform.position);
            if (this is Railshot railshot)
            {
                HitEffect(collision.gameObject.transform.position);
            }

            if (projData.destroyOnCollision)
                CollisionHit();
        }

        // Enemy Collision 
        else if (attacking_layer == 6 && collision.gameObject.TryGetComponent<Enemy>(out var enemy))
        {
            enemy.TakeDamage(projData.damage);
            CheckForStatusEffect(enemy);
            HitEffect(transform.position);
            if (this is Railshot railshot)
            {
                HitEffect(collision.gameObject.transform.position);
            }

            if (projData.destroyOnCollision)
                CollisionHit();
        }

        // Level Collision
        if ((collision.gameObject.layer == 0 && projData.destroyOnCollision) || collision.gameObject.layer == 9)
        {
            //print(collision.gameObject.name);            
            HitEffect(transform.position);
            CollisionHit();
        }
    }

    public void CheckForStatusEffect(Entity entity)
    {
        if (projData.slows)
            entity.ApplySlowEffect(projData.slowDuration, projData.slowdownFactor);

        if (projData.setsOnFire)
            entity.ApplyFireEffect(projData.burnDuration, projData.burnDamage);
    }

    public override void EntityDie()
    {
        Destroy(gameObject);
    }

    public virtual void CollisionHit()
    {
        collisionHit = true;
        spriteRenderer.enabled = false;
        projectileCollider.enabled = false;
    }

    public void HitEffect(Vector2 position)
    {
        if (!projData.hitEffect) return;

        GameObject effect = Instantiate(projData.hitEffect, position, Quaternion.identity, LevelManager.current_level.EntityList.transform);

        audioManager.PlayAudioSource("Impact");

        Destroy(effect, 0.1f);
    }
}

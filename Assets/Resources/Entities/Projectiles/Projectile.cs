using System;
using Unity.VisualScripting;
using UnityEngine;

public abstract class Projectile : Entity
{
    public SpriteRenderer spriteRenderer;
    public Collider2D projectileCollider;

    [NonSerialized] public ProjectileData projData;
    [NonSerialized] public LayerMask attacking_layer = 0;
    private bool collisionHit = false;
    private float timer = 0f;

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
            bool blocked = false;

            if (player.isShielding)
            {
                // Player's forward direction (the direction they are facing)
                Vector2 playerForward = -player.firePoint.right.normalized;

                // Direction from player to projectile
                Vector2 toProjectile = (transform.position - player.transform.position).normalized;

                float dot = Vector2.Dot(playerForward, toProjectile);

                blocked = dot > Mathf.Cos(45f * Mathf.Deg2Rad);
            }

            if (!blocked)
            {
                player.TakeDamage(projData.damage);
                CheckForEffect(player);
            }
            else Debug.Log("Projectile Blocked");

            // Destroy projectile if necessary (blocked or hit)
            HitEffect(collision.transform.position);
            if (projData.destroyOnCollision)
                CollisionHit();
        }

        // Enemy Collision 
        else if (attacking_layer == 6 && collision.gameObject.TryGetComponent<Enemy>(out var enemy))
        {
            enemy.TakeDamage(projData.damage);
            CheckForEffect(enemy);
            HitEffect(collision.transform.position);

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

    public void CheckForEffect(Entity entity)
    {
        if (projData.shocks)
        {
            entity.ApplyShockEffect(projData.shockDuration, projData.slowDownFactor);
        }
        else if (projData.setsOnFire)
        {
            entity.ApplyOnFireEffect(projData.burnDuration, projData.burnDamage);
        }
        else
        {
            return;
        }
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

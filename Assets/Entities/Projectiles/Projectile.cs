using System;
using UnityEngine;

public abstract class Projectile : Entity
{
    [NonSerialized] public ProjectileData projData;
    [NonSerialized] public LayerMask attacking_layer = 0;

    public override void Start()
    {
        base.Start();
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        // Player Collision
        if (attacking_layer == 7 && collision.gameObject.TryGetComponent<Player>(out var player))
        {
            bool blocked = false;

            if (player.getIsBlocking())
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
            }
            else Debug.Log("Projectile Blocked");

            // Destroy projectile if necessary (blocked or hit)
            HitEffect(collision.transform.position);
            if (projData.destroyOnCollision)
                EntityDie();
        }

        // Enemy Collision 
        else if (attacking_layer == 6 && collision.gameObject.TryGetComponent<Enemy>(out var enemy))
        {
            enemy.TakeDamage(projData.damage);
            HitEffect(collision.transform.position);
            
            if (projData.destroyOnCollision)
                EntityDie();
        }

        // Level Collision
        if (collision.gameObject.layer == 0 && projData.destroyOnCollision)
        {
            //print(collision.gameObject.name);            
            HitEffect(transform.position);
            EntityDie();
        }
    }


    public override void EntityDie()
    {
        Destroy(gameObject);
    }
    
    public void HitEffect(Vector2 position)
    {
        GameObject effect = Instantiate(projData.hitEffect, position, Quaternion.identity, LevelManager.current_level.EntityList.transform);

        Destroy(effect, 0.1f);
    }
}

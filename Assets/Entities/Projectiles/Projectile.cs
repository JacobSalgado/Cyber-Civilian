using System;
using UnityEngine;

public abstract class Projectile : Entity
{
    [Header("==Projectile GameObjects==")]
    public GameObject hitEffect;
    public LayerMask attacking_layer = 0;

    [NonSerialized] public float force;
    [NonSerialized] public int damage;

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (attacking_layer == 7 && collision.gameObject.TryGetComponent<Player>(out var player))
        {
            player.TakeDamage(damage);
            ProjectileExplode();
        }
        else if (attacking_layer == 6 && collision.gameObject.TryGetComponent<Enemy>(out var enemy))
        {
            enemy.TakeDamage(damage);
            ProjectileExplode();
        }
        
        // colliding with the level
        if (collision.gameObject.layer == 0 && !collision.gameObject.TryGetComponent<Entity>(out _))
        {
            //print(collision.gameObject.name);
            ProjectileExplode();
        }
    }

    public void ProjectileExplode()
    {
        GameObject effect = Instantiate(hitEffect, transform.position, Quaternion.identity, LevelManager.current_level.EntityList.transform);

        Destroy(effect, 0.1f);
        Destroy(gameObject);
    }
}

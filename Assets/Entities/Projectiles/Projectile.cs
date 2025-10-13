using System;
using UnityEngine;

public abstract class Projectile : Entity
{
    //[Header("==Projectile GameObjects==")]
    // assigned by weapon
    [NonSerialized] public ProjectileData projData;
    [NonSerialized] public LayerMask attacking_layer = 0;

    public override void Start()
    {
        base.Start();
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (attacking_layer == 7 && collision.gameObject.TryGetComponent<Player>(out var player))
        {
            player.TakeDamage(projData.damage);
            HitEffect(collision.transform.position);

            if (projData.destroyOnCollision)
                EntityDie();
        }
        else if (attacking_layer == 6 && collision.gameObject.TryGetComponent<Enemy>(out var enemy))
        {
            enemy.TakeDamage(projData.damage);
            HitEffect(collision.transform.position);
            
            if (projData.destroyOnCollision)
                EntityDie();
        }

        // colliding with the level
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

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
            EntityDie();
        }
        else if (attacking_layer == 6 && collision.gameObject.TryGetComponent<Enemy>(out var enemy))
        {
            enemy.TakeDamage(projData.damage);
            EntityDie();
        }

        // colliding with the level
        if (collision.gameObject.layer == 0)
        {
            //print(collision.gameObject.name);
            EntityDie();
        }
    }
    
    public override void EntityDie()
    {
        GameObject effect = Instantiate(projData.hitEffect, transform.position, Quaternion.identity, LevelManager.current_level.EntityList.transform);

        Destroy(effect, 0.1f);
        Destroy(gameObject);
    }
}

using System;
using UnityEngine;

public abstract class Projectile : Entity
{
    [Header("==Projectile GameObjects==")]
    public GameObject hitEffect;

    [NonSerialized] public float force;
    [NonSerialized] public int damage;

    public void OnTriggerEnter2D(Collider2D collision)
    {
        //print(hurtbox.gameObject.layer);
        if (gameObject.layer == 7 && collision.gameObject.TryGetComponent<Player>(out var player))
        {
            player.TakeDamage(damage);
        }
        else if (gameObject.layer == 6 && collision.gameObject.TryGetComponent<Enemy>(out var enemy))
        {
            enemy.TakeDamage(damage);
        }

        if (collision.gameObject.layer == 0)
        {
            GameObject effect = Instantiate(hitEffect, transform.position, Quaternion.identity);
            Destroy(effect, 0.1f);
            Destroy(gameObject);
        }
    }
}

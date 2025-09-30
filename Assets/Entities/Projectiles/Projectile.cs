using System;
using UnityEngine;

public class Projectile : Entity
{
    public GameObject hitEffect;
    public int damage;
    public float force;

    public override void InitializeStates()
    {
        AddState("Idle", new ProjectileIdle(this));
        AddState("Travel", new ProjectileTravel(this));

        ChangeState("Travel");
    }

    public override void Start()
    {
        base.Start();
        InitializeStates();
    }

    public override void OnCollisionEnter2D(Collision2D collision)
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

        if (!collision.gameObject.TryGetComponent<Projectile>(out var proj))
        {
            GameObject effect = Instantiate(hitEffect, transform.position, Quaternion.identity);
            Destroy(effect, 0.1f);
            Destroy(gameObject);
        }
    }
}

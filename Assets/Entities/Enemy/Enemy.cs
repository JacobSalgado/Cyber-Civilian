using UnityEngine;

public class Enemy : Entity
{
    public Transform target; // following the player
    public GameObject proj;
    public Transform firePoint;

    public override void InitializeStates()
    {
        AddState("Idle", new EnemyIdle(this));
        AddState("Move", new EnemyMove(this));

        ChangeState("Idle");
    }

    private void Start()
    {
        InitializeStates();
    }

    public void Shoot()
    {
        Projectile proj = Instantiate(this.proj, firePoint.position, firePoint.rotation).GetComponent<Projectile>();
    }

    public override void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.TryGetComponent<Projectile>(out var proj)
        ) {
            TakeDamage(proj.damage);
        }
    }
}

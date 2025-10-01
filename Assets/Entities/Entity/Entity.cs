using System.Security.Cryptography;
using UnityEngine;

public abstract class Entity : StateManager
{
    [Header("Entity GameObjects")]
    public Rigidbody2D rigidBody;
    public AudioSource SFXPlayer;
    public EntityData entityData;
    public HealthBar healthBar;
    public PolygonCollider2D hurtbox;

    public bool invincibility = false;

    public abstract void InitializeStates();

    public void TakeDamage(int damageTaken)
    {
        if (invincibility || entityData.currentHealth <= 0) return;

        SetHealth(entityData.currentHealth - damageTaken);
    }

    public void SetHealth(int new_health)
    {
        if (new_health > entityData.maxHealth)
            entityData.currentHealth = entityData.maxHealth;
        else if (new_health < 0)
        {
            entityData.currentHealth = 0;
            //ChangeState("Death");
        }
        else
            entityData.currentHealth = new_health;

        if (healthBar != null)
        {
            healthBar.UpdateHealthBar();
        }
    }

    public void RotateToDirection(Vector2 dir)
    {
        float angleRad = Mathf.Atan2(dir.y, dir.x);
        float angleDeg = angleRad * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, angleDeg + 180f);
    }

    public virtual void FixedUpdate()
    {
        current_state.UpdateState();
    }

    public virtual void OnCollisionEnter2D(Collision2D collision)
    {
        
    }
}

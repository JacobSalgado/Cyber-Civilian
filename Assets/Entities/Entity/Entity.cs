using System.Security.Cryptography;
using UnityEngine;

public abstract class Entity : StateManager
{
    [Header("Entity GameObjects")]
    public Rigidbody2D rigidBody;
    public AudioSource SFXPlayer;
    public EntityData entityData;
    public HealthBar healthBar;

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

        
            healthBar.UpdateHealthBar();
    }

    public virtual void FixedUpdate()
    {
        current_state.UpdateState();
    }
}

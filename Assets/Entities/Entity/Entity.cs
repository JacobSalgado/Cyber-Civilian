using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public abstract class Entity : StateManager
{
    [Header("==Entity GameObjects and Vars==")]
    public Rigidbody2D rigidBody;
    public EntityData entityData;
    public Slider healthBar;
    public AudioManager audioManager;
    public bool invincibility = false;

    /* VIRTUAL/ABSTRACT ENTITY FUNCTIONS */
    public virtual void InitializeStates() { }
    public abstract void EntityDie();

    public virtual void Start()
    {
        // make a copy of the entityData
        if (entityData != null)
        {
            entityData = Instantiate(entityData);
            audioManager.InitializeAudioDictionary(entityData.SFXNames, entityData.SFX);
        }
    }

    public virtual void FixedUpdate()
    {
        current_state.UpdateState();

        // health checks
        if (entityData != null && entityData.currentHealth <= 0 && entityData.maxHealth != 0)
        {
            EntityDie();
        }
    }

    /* GENERAL ENTITY FUNCTIONS */
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
            UpdateHealthBar();
        }
    }

    public Vector2 GetDirectionToPosition(Vector2 point)
    {
        Vector2 direction = (point - (Vector2) transform.position).normalized;
        return direction;
    }

    public void RotateToDirection(Vector2 dir)
    {
        float angleRad = Mathf.Atan2(dir.y, dir.x);
        float angleDeg = angleRad * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, angleDeg + 180f);
    }

    public void UpdateHealthBar()
    {
        healthBar.maxValue = entityData.maxHealth;
        healthBar.value = entityData.currentHealth;
    }

    public void ShootWeapon(GameObject weapon, InputActionReference fireAction, Transform firePoint, int collision_layer)
    {
        if (weapon != null)
            weapon.GetComponent<Weapon>().Shoot(fireAction, firePoint, collision_layer);
    }
}

using UnityEngine;
using UnityEngine.InputSystem;

public abstract class Entity : StateManager
{
    [Header("==Entity GameObjects and Vars==")]
    public Rigidbody2D rigidBody;
    public AudioSource SFXPlayer;
    public EntityData entityData;
    public HealthBar healthBar;

    public bool invincibility = false;

    public virtual void InitializeStates()
    {

    }

    // dictionary: asset store or scriptable objects
    public virtual void Start()
    {
        // make a copy of the entityData
        if (entityData != null)
        {
            entityData = Instantiate(entityData);
            if (healthBar != null)
            {
                healthBar.entityData = entityData;
            }
        }
    }

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

    public virtual void FixedUpdate()
    {
        current_state.UpdateState();
    }

    public void ShootWeapon(GameObject weapon, InputActionReference fireAction, Transform firePoint, int collision_layer)
    {
        weapon.GetComponent<Weapon>().Shoot(fireAction, firePoint, collision_layer);
    }
}

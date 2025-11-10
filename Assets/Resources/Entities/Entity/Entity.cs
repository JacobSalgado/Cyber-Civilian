using System;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public abstract class Entity : StateManager
{
    [Header("==Entity GameObjects and Vars==")]
    public Animator animator;
    public Rigidbody2D rigidBody;
    public EntityData entityData;
    public Slider healthBar;
    public AudioManager audioManager;
    public AudioEffect[] audioEffects;
    public bool invincibility = false;
    public bool isOnFire = false;
    public int fireDamage;
    public float slowDownFactor;
    public bool isShocked = false;
    private float originalMoveSpeed;

    [NonSerialized] public bool isDamaged = false;
    [NonSerialized] public bool isDead = false;

    /* VIRTUAL/ABSTRACT ENTITY FUNCTIONS */
    public abstract void EntityDie();
    public virtual void InitializeStates() { }
    public virtual void GotDamaged() { }

    public virtual void Start()
    {
        // make a copy of the entityData
        if (entityData != null)
            entityData = Instantiate(entityData);

        audioManager.InitializeAudioDictionary(audioEffects);
    }

    public virtual void FixedUpdate()
    {
        rigidBody.angularVelocity = 0f;
        current_state.UpdateState();

        // health checks
        if (entityData != null && entityData.currentHealth <= 0 && entityData.maxHealth != 0)
        {
            EntityDie();
        }

        // Status Effects
        if (isOnFire)
        {
            TakeDamage(fireDamage);
        }
    }

    /* GENERAL ENTITY FUNCTIONS */
    public void TakeDamage(int damageTaken)
    {
        if (invincibility || entityData.currentHealth <= 0) return;

        SetHealth(entityData.currentHealth - damageTaken);

        isDamaged = true;
        GotDamaged();
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

    public void ShootWeapon(GameObject weapon, InputActionReference fireAction, Transform firePoint, int collision_layer, string fireSFX = "")
    {
        if (weapon != null)
        {
            weapon.GetComponent<Weapon>().Shoot(fireAction, firePoint, collision_layer, fireSFX);
        }
    }

    public bool PlayFootsteps(float deltaCount, float timeToStep, int stepCounter)
    {
        if (deltaCount > timeToStep)
        {
            audioManager.PlayAudioSource($"Footsteps{stepCounter}");
            return true;
        }
        return false;
    }

    public void PlayAnim(string name)
    {
        animator.Play(name);
    }

    public void ApplyOnFireEffect(float duration, int damage)
    {
        if (isOnFire) return;
        fireDamage = damage;
        isOnFire = true;
        StartCoroutine(FireEffectTimer(duration));
    }

    private IEnumerator FireEffectTimer(float duration)
    {
        yield return new WaitForSeconds(duration);
        isOnFire = false;
    }

    public void ApplyShockEffect(float duration, float speedReduction)
    {
        if (isShocked) return;
        slowDownFactor = speedReduction;
        isShocked = true;
        originalMoveSpeed = entityData.moveSpeed;
        entityData.moveSpeed *= slowDownFactor;
        StartCoroutine(ShockEffectTimer(duration));
    }
    
    private IEnumerator ShockEffectTimer(float duration)
    {
        yield return new WaitForSeconds(duration);
        entityData.moveSpeed = originalMoveSpeed;
        isShocked = false;
    }
}

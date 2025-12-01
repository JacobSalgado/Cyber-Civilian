using System;
using System.Collections;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public abstract class Entity : StateManager
{
    [Header("==Entity GameObjects==")]
    public Animator animator;
    public Rigidbody2D rigidBody;
    public EntityData entityData;
    public Slider healthBar;
    public AudioManager audioManager;
    public AudioEffect[] audioEffects;

    [Header("==VFX==")]
    [SerializeField] private GameObject vfxHolder;
    [SerializeField] private ParticleSystem fireEffect;
    [SerializeField] private ParticleSystem slowEffect;
    [SerializeField] protected GameObject deathEffectPrefab;

    [NonSerialized] public bool isDamaged = false;
    [NonSerialized] public bool isDead = false;

    // Movement Variables
    [NonSerialized] public Vector2 moveVelocity = Vector2.zero;
    [NonSerialized] public Vector2 pushedVelocity = Vector2.zero;
    private Vector2 finalMovementVelocity = Vector2.zero;

    /* Entity Status Effects */
    // push/knockback
    [NonSerialized] public bool pushed = false;
    [NonSerialized] public float pushedTime = 0f;
    private float pushTimer = 0.0f;

    // invicibility
    [NonSerialized] public bool invincibility = false;

    // fire over time
    [NonSerialized] public bool isOnFire = false;
    [NonSerialized] public int fireDamage;

    // slow
    [NonSerialized] public bool isSlowed = false;
    [NonSerialized] public float slowdownFactor = 0.5f;


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
        vfxHolder.transform.rotation = Quaternion.identity;
        current_state.UpdateState();

        // health checks
        if (isOnFire) {
            // fireEffect.transform.position = gameObject.transform.position;
            TakeDamage(Mathf.CeilToInt(fireDamage * Time.deltaTime));
        }
        
        if (entityData != null && entityData.currentHealth <= 0 && entityData.maxHealth != 0)
        {
            Destroy(fireEffect);
            EntityDie();
        }

        // movement checks
        if (pushed)
        {
            pushTimer += Time.deltaTime;

            // decrease knockback velocity
            rigidBody.linearVelocity = pushedVelocity;
            pushedVelocity *= 0.85f;

            if (pushTimer > pushedTime)
            {
                pushTimer = 0f;
                pushed = false;
            }
        }
        else {
            finalMovementVelocity = moveVelocity;
            if (isSlowed)
                finalMovementVelocity *= slowdownFactor;
            rigidBody.linearVelocity = finalMovementVelocity;
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

    public void ChangeSpriteAlpha(SpriteRenderer sr, float new_alpha)
    {
        Color tempColor = sr.color;
        tempColor.a = new_alpha;
        sr.color = tempColor;
    }

    public void ApplyFireEffect(float duration, int damage)
    {
        if (isOnFire) return;

        fireDamage = damage;
        isOnFire = true;
        StartCoroutine(FireEffectTimer(duration));
        fireEffect.Play();
    }

    private IEnumerator FireEffectTimer(float duration)
    {
        yield return new WaitForSeconds(duration);
        isOnFire = false;
        fireEffect.Stop();
    }

    public void ApplySlowEffect(float duration, float slowdownFactor)
    {
        if (isSlowed) return;

        isSlowed = true;
        this.slowdownFactor = slowdownFactor;
        StartCoroutine(SlowEffectTimer(duration));
        slowEffect.Play();
    }
    
    private IEnumerator SlowEffectTimer(float duration)
    {
        yield return new WaitForSeconds(duration);
        isSlowed = false;
        slowEffect.Stop();
    }
}

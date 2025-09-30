using UnityEngine;

public abstract class Entity : StateManager
{
    [Header("Entity GameObjects and Vars")]
    public Rigidbody2D rigidBody;
    public AudioSource SFXPlayer;
    public EntityData entityData;
    public HealthBar healthBar;
    public PolygonCollider2D hurtbox;

    public Transform firePoint;
    [SerializeField] protected float fireTimer = 0f;
    [SerializeField] protected float fireRate = 5f;

    public bool invincibility = false;

    public abstract void InitializeStates();

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

    public void ShootProjectile(GameObject proj, Transform fire_point, int collision_layer)
    {
        string ignored_layer = collision_layer == 6 ? "Enemy Attacks" : "Player Attacks";
        LayerMask layer = LayerMask.GetMask(ignored_layer);

        Projectile projectile = Instantiate(proj, fire_point.position, fire_point.rotation).GetComponent<Projectile>();
        projectile.gameObject.layer = collision_layer;
        projectile.rigidBody.excludeLayers = layer;

    }
}

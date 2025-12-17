using UnityEngine;

public class CyberBossShield : MonoBehaviour
{
    private enum ShieldState
    {
        Healthy,
        Damaged,
        Critical,
        Broken
    }

    private ShieldState currentState = ShieldState.Healthy;


    [Header("==Shield GameObjects==")]
    [SerializeField] private CyberBoss _cyberBoss;
    [SerializeField] SpriteRenderer spriteRenderer;

    [Header("==Shield Stats==")]
    [SerializeField] private int maxShieldHealth = 10000;
    private int currentShieldHealth;

    // shield checks since there was overlapping issues
    private bool inCriticalCondition = false;

    // Shield Colors
    Color red = new(1f, 0.25f, 0.25f, 0.5f);  // critical condition
    Color yellow = new(1f, 0.9f, 0.2f, 0.5f); // damaged shield
    Color blue = new(0.2f, 0.6f, 1f, 0.5f); // shield is in good condition

    void Start()
    {
        spriteRenderer.enabled = false;
        currentShieldHealth = maxShieldHealth;
        spriteRenderer.color = blue; // initial color
    }

    public void Shield(bool activate)
    {
        if (activate)
        { 
            spriteRenderer.enabled = true;
            _cyberBoss.EquipShield();
        }
        else
        {
            spriteRenderer.enabled = false;
            _cyberBoss.UnequipShield();
        }
    }

    public void TakeShieldDamage(int damage)
    {
        if (currentState == ShieldState.Broken)
            return;
        
        currentShieldHealth -= damage;
        float healthPercentage = (float)currentShieldHealth / maxShieldHealth;

        // CHANGE SHIELD COLOR BASED ON HEALTH PERCENTAGE
        if (healthPercentage <= 0f)
        {
            BreakShield();
            return;
        }

        if (healthPercentage <= 0.375f && currentState != ShieldState.Critical)
            EnterCriticalState();
        else if (healthPercentage <= 0.75f && currentState != ShieldState.Damaged && !inCriticalCondition)
            EnterDamagedState();
    }

    public void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.gameObject.TryGetComponent<Projectile>(out var proj) && proj.attacking_layer == 6)
        {
            TakeShieldDamage(proj.projData.damage);
            proj.HitEffect(proj.gameObject.transform.position);
            proj.CollisionHit();   
        }
    }

    // healthy to damaged
    private void EnterDamagedState()
    {
        currentState = ShieldState.Damaged;
        spriteRenderer.color = yellow;

        // healthy shield shatters
        _cyberBoss.shieldBrokenHealthyEffect.Play();
    }

    // damaged to crtical
    private void EnterCriticalState()
    {
        currentState = ShieldState.Critical;
        spriteRenderer.color = red;
        inCriticalCondition = true;

        // damgaged shield shatters
        _cyberBoss.shieldBrokenDamagedEffect.Play();
    }

    // critical to broken
    private void BreakShield()
    {
        currentState = ShieldState.Broken;

        spriteRenderer.enabled = false;
        gameObject.SetActive(false);

        // Shield completely shatters
        _cyberBoss.shieldBrokenCriticalEffect.Play();

        _cyberBoss.UnequipShield();

        // TODO
        // Explosion effect when the shield breaks
    }
}

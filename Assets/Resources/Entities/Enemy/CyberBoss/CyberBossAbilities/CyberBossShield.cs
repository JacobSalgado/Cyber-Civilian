using UnityEngine;

public class CyberBossShield : MonoBehaviour
{
    [Header("==Shield GameObjects==")]
    [SerializeField] private CyberBoss _cyberBoss;
    [SerializeField] SpriteRenderer spriteRenderer;

    [Header("==Shield Stats==")]
    [SerializeField] private int maxShieldHealth = 10000;
    private int currentShieldHealth;

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

    private void BreakShield()
    {
        spriteRenderer.enabled = false;
        gameObject.SetActive(false);
        _cyberBoss.UnequipShield();
    }

    public void TakeShieldDamage(int damage)
    {
        currentShieldHealth -= damage;

        float healthPercentage = (float)currentShieldHealth / maxShieldHealth;

        // CHANGE SHIELD COLOR BASED ON HEALTH PERCENTAGE
        if (healthPercentage <= 0.25f)
            spriteRenderer.color = red; // critical color
        else if (healthPercentage <= 0.5f)
            spriteRenderer.color = yellow; // damaged color
    
        if (currentShieldHealth <= 0)
            BreakShield();
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
}

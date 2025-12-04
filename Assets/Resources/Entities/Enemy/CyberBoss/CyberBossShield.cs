using UnityEngine;

public class CyberBossShield : MonoBehaviour
{
    [SerializeField] private CyberBoss _cyberBoss;
    [SerializeField] SpriteRenderer spriteRenderer;

    [Header("Shield Stats")]
    [SerializeField] private int maxShieldHealth = 10000;
    private int currentShieldHealth;
    private bool shieldActive = false;

    // Shield Colors
    Color red = new Color(1f, 0.25f, 0.25f, 0.5f);  // critical condition
    Color yellow = new Color(1f, 0.9f, 0.2f, 0.5f); // damaged shield
    Color blue = new Color(0.2f, 0.6f, 1f, 0.5f); // shield is in good condition


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        spriteRenderer.enabled = false;
        currentShieldHealth = maxShieldHealth;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Shield(bool activate)
    {
        shieldActive = activate;

        if (activate)
        { 
            spriteRenderer.enabled = true;
            _cyberBoss.EquipShield();
            Debug.Log("Shield activated");
        }
        else
        {
            spriteRenderer.enabled = false;
            _cyberBoss.UnequipShield();
        }
    }

    private void BreakShield()
    {
        shieldActive = false;
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
        else
            spriteRenderer.color = blue; // initial color

        if (currentShieldHealth <= 0)
        {
            BreakShield();
        }
    }

    public void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.gameObject.TryGetComponent<Projectile>(out var proj))
        {
            if (proj.attacking_layer == 6)
            {
                TakeShieldDamage(proj.projData.damage);
                //TakeShieldDamage(50); // For testing purposes, each projectile does 50 damage to the shield
                Debug.Log("Shield hit! Reached this point");
                proj.CollisionHit();
            }
        }
    }

    /*public bool IsShieldBlocking(Vector2 vectorToBlock)
    {
        Vector2 cyberBossForward = -_cyberBoss.firePoint.right.normalized;
        float dot = Vector2.Dot(vectorToBlock, cyberBossForward);
        return dot > 0.5;
    }*/
}

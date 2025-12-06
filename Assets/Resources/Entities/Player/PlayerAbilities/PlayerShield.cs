using UnityEngine;

public class PlayerShield : MonoBehaviour
{
    [Header("==Necessary GameObjects==")]
    [SerializeField] private Player _player;
    [SerializeField] CircleCollider2D aoe;
    [SerializeField] SpriteRenderer spriteRenderer;

    [Header("==Shield Values==")]
    [SerializeField] private int shieldDrainRate = 50;

    [Header("Shield Bash Values")]
    [SerializeField] private int bashDamage = 20;
    [SerializeField] private float bashSlowStrength = 0.7f;
    [SerializeField] private float bashSlowDuration = 2f;
    [SerializeField] private int bashFireDamage = 10;
    [SerializeField] private float bashFireDuration = 2.5f;

    void Start()
    {
        aoe.enabled = false;
        spriteRenderer.enabled = false;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        gameObject.transform.SetLocalPositionAndRotation(Vector2.zero, Quaternion.identity);
        if (_player.isShielding)
        {
            if (_player.currentEnergy <= 0) Shield(false);
            else _player.currentEnergy -= shieldDrainRate * Time.deltaTime;
        }
    }

    public void Shield(bool activate, bool playAudio = true)
    {
        if (activate) {
            aoe.enabled = true;
            spriteRenderer.enabled = true;
            _player.EquipShield(playAudio);
        }
        else {
            aoe.enabled = false;
            spriteRenderer.enabled = false;
            _player.UnequipShield(playAudio);
        }
    }

    public void OnTriggerEnter2D(Collider2D collider)
    {
        if (_player.isDashing && collider.gameObject.TryGetComponent<Enemy>(out var enemy))
        {
            // Shield Bash Logic
            Vector2 toEnemy = -enemy.GetDirectionToPosition(_player.transform.position);

            if (IsShieldBlocking(toEnemy))
            {
                // play audio
                enemy.TakeDamage(bashDamage);
                enemy.ApplyFireEffect(bashFireDuration, bashFireDamage);
                enemy.ApplySlowEffect(bashSlowDuration, bashSlowStrength);
            }
        }
        else if (collider.gameObject.TryGetComponent<Projectile>(out var proj))
        {
            if (proj.attacking_layer == 7)
            {
                // Direction from player to projectile
                Vector2 toProjectile = -proj.GetDirectionToPosition(_player.transform.position);

                if (IsShieldBlocking(toProjectile))
                {
                    // play audio
                    proj.HitEffect(proj.gameObject.transform.position);
                    proj.CollisionHit();
                }
            }
        }
    }

    public bool CanActivate()
    {
        return _player.currentEnergy - shieldDrainRate >= 0;
    }

    public bool IsShieldBlocking(Vector2 vectorToBlock)
    { 
        Vector2 playerForward = -_player.firePoint.right.normalized;
        float dot = Vector2.Dot(vectorToBlock, playerForward);
        return dot > 0.5;
    }
}

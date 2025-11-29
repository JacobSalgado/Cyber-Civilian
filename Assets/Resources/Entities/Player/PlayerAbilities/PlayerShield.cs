using UnityEngine;

public class PlayerShield : MonoBehaviour
{
    [Header("==Necessary GameObjects==")]
    [SerializeField] private Player _player;
    [SerializeField] CircleCollider2D aoe;
    [SerializeField] SpriteRenderer spriteRenderer;

    [Header("==Shield Values==")]
    [SerializeField] private int shieldDrainRate = 50;
    [SerializeField] private float shieldShockDuration = 2f;
    [SerializeField] private float shieldShockStrength = 0.5f;
    [SerializeField] private int shieldFireDamage = 1;
    [SerializeField] private float shieldFireDuration = 3f;

    void Start()
    {
        aoe.enabled = false;
        spriteRenderer.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        gameObject.transform.SetLocalPositionAndRotation(Vector2.zero, Quaternion.identity);
        if (_player.isShielding)
        {
            if (_player.currentEnergy < 0) Shield(false);
            else _player.currentEnergy -= Mathf.RoundToInt(shieldDrainRate * Time.deltaTime);
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

    public bool CanActivate()
    {
        return _player.currentEnergy - shieldDrainRate >= 0;
    }

    public void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.gameObject.TryGetComponent<Projectile>(out var proj))
        {
            if (proj.attacking_layer == 7)
            {
                // Direction from player to projectile
                Vector2 toProjectile = -proj.GetDirectionToPosition(_player.transform.position);

                if (IsShieldBlocking(toProjectile))
                {
                    proj.HitEffect(proj.gameObject.transform.position);
                    proj.CollisionHit();
                }
            }
        }
    }

    public bool IsShieldBlocking(Vector2 vectorToBlock)
    { 
        Vector2 playerForward = -_player.firePoint.right.normalized;
        float dot = Vector2.Dot(vectorToBlock, playerForward);
        print(dot);
        return dot > 0.5;
    }

    // public void OnCollisionEnter2D(Collision2D collision)
    // {
    //     //Debug.Log("Collided with " + collision.gameObject.name);
    //     // if (isShieldBlocking)
    //     // {
    //     //     if (collision.gameObject.TryGetComponent<Enemy>(out var enemy))
    //     //     {
    //     //         Debug.Log("Applied shock from shield to enemy");
    //     //         enemy.ApplyShockEffect(shiledShockDuration, shieldSlowDownStrength);
    //     //         enemy.ApplyOnFireEffect(shiledFireDuration, shieldFireDamage);
    //     //     }
    //     // }
    // }
}

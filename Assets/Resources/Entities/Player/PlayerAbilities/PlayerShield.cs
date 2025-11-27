using System;
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
        if (_player.isShielding)
        {
            if (_player.currentEnergy < 0) Shield(false);
            else _player.currentEnergy -= (int) Math.Ceiling(shieldDrainRate * Time.deltaTime);
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
                // Player's forward direction (the direction they are facing)
                Vector2 playerForward = -_player.firePoint.right.normalized;

                // Direction from player to projectile
                Vector2 toProjectile = proj.GetDirectionToPosition(_player.transform.position);

                float dot = Vector2.Dot(toProjectile, playerForward);

                bool blocked = dot < -0.5;

                if (blocked)
                {
                    proj.HitEffect(proj.gameObject.transform.position);
                    proj.CollisionHit();
                }
            }
        }
    }
}

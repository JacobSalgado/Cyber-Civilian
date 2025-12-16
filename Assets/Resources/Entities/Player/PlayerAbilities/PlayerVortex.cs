using System;
using UnityEngine;

public class PlayerVortex : MonoBehaviour
{
    [SerializeField] private Player _player;
    [SerializeField] CircleCollider2D aoe;
    [SerializeField] SpriteRenderer spriteRenderer;
    [SerializeField] private int vortexDrainRate = 60;
    [SerializeField] private int absorbCost = 40;
    [SerializeField] private int maxAbsorbedProjectiles = 25;  // if exceeded, vortex will explode and player will take selfExplosionDamage
    [SerializeField] private int selfExplosionDamage = 250;
    [SerializeField] private float damagedAbsorbedToMultiplierPercentage = 0.1f;

    private int absorbedCount = 0;
    private float damageMultiplier = 1f;

    void Start()
    {
        aoe.enabled = false;
        spriteRenderer.enabled = false;
    }

    void FixedUpdate()
    {
        gameObject.transform.SetLocalPositionAndRotation(Vector2.zero, Quaternion.identity);
        if (_player.isVortexing)
        {
            if (_player.currentEnergy <= 0) EmitVortex(absorbedCount > 0);
            else _player.currentEnergy -= vortexDrainRate * Time.deltaTime;
        } 
    }

    public void EmitVortex(bool applyMulitpler = true, bool playAudio = true)
    {
        if (!aoe.enabled)
        {
            aoe.enabled = true;
            spriteRenderer.enabled = true;
            _player.StartVortex(playAudio);
        }
        else
        {            
            // apply damage multiplier
            if (!applyMulitpler) damageMultiplier = 1f;
            
            _player.StopVortex(damageMultiplier, playAudio);

            // reset values
            aoe.enabled = false;
            spriteRenderer.enabled = false;
            absorbedCount = 0;
            damageMultiplier = 1f;
        }
    }

    public bool CanActivate()
    {
        return _player.currentEnergy - vortexDrainRate >= 0;
    }

    public void AbsorbProjectile(int projDamage)
    {
        absorbedCount++;
    
        // make a limit to how much can be absorbed and make the player take damage if it exceeds the limit
        if (absorbedCount > maxAbsorbedProjectiles)
        {
            _player.TakeDamage(selfExplosionDamage);
            absorbedCount = 0;
            EmitVortex(false);
            return;
        }

        // update multiplier based on damage from proj damage
        damageMultiplier += damagedAbsorbedToMultiplierPercentage * projDamage * 0.01f;
        float damageMultiplierText = MathF.Round(damageMultiplier * 100f - 100); // rounding to nearest tenth
        _player.hud.UpdateVortexMultiplierText(damageMultiplierText);

        // TODO: add visual/audio feedback
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.TryGetComponent<Projectile>(out var proj))
        {
            if (proj.attacking_layer == 7)
            {
                //Debug.Log("Projectile absorbed by vortex");
                _player.currentEnergy -= absorbCost;

                AbsorbProjectile(proj.projData.damage);
                proj.CollisionHit();
            }
        }
    }
}

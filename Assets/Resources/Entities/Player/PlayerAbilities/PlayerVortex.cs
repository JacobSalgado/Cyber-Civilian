using System;
using UnityEngine;

public class PlayerVortex : MonoBehaviour
{
    [SerializeField] private Player _player;
    [SerializeField] CircleCollider2D aoe;
    [SerializeField] SpriteRenderer spriteRenderer;
    [SerializeField] private int selfExplosionDamage = 250;
    [SerializeField] private int maxAbsorbedProjectiles = 25;  // if exceeded, vortex will explode and player will take selfExplosionDamage
    [SerializeField] private float damageMultiplierPerProjectile = 0.2f;

    private int absorbedCount = 0;
    private float damageMultiplier = 1f;

    void Start()
    {
        aoe.enabled = false;
        spriteRenderer.enabled = false;
    }

    void Update()
    {
        gameObject.transform.localPosition = Vector2.zero;
        gameObject.transform.localRotation = Quaternion.identity;
        if (_player.isVortexing)
        {
            if (_player.currentEnergy <= 0)
            {
                EmitVortex(true);
                _player.chargeMeter.TurnOffMeter();
                return;    
            }
            else {
                _player.currentEnergy -= (int) Math.Ceiling(_player.vortexDrainRate * Time.deltaTime);

                _player.chargeMeter.UpdateMeter(_player.currentEnergy);
            }
        } 
    }

    public void EmitVortex(bool applyMulitpler = true, bool playAudio = true)
    {
        if (!aoe.enabled)
        {
            aoe.enabled = true;
            spriteRenderer.enabled = true;
            _player.StartVortex(playAudio);

            // TODO: start visual effect
        }
        else
        {            
            // apply damage multiplier
            if (applyMulitpler)
            {
                damageMultiplier += absorbedCount * damageMultiplierPerProjectile;
                _player.StopVortex(damageMultiplier, playAudio);
            }
            else _player.isVortexing = false;

            // reset values
            aoe.enabled = false;
            spriteRenderer.enabled = false;
            absorbedCount = 0;
            damageMultiplier = 1f;
            
            // TODO: stop visual effect
        }
    }

    public void AbsorbProjectile()
    {
        absorbedCount++;
    
        // make a limit to how much can be absorbed and make the player take damage if it exceeds the limit
        if (absorbedCount > maxAbsorbedProjectiles)
        {
            _player.TakeDamage(selfExplosionDamage);
            absorbedCount = 0;
            EmitVortex(true);
        }

        // TODO: add visual/audio feedback

        Debug.Log($"Absorbed projectile! Count: {absorbedCount}");
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.TryGetComponent<Projectile>(out var proj))
        {
            if (proj.attacking_layer == 7)
            {
                Debug.Log("Projectile absorbed by vortex");
                _player.currentEnergy -= _player.vortexCost;
                AbsorbProjectile();
                proj.CollisionHit();
            }
        }
    }
}

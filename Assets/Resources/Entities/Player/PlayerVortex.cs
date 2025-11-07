using UnityEngine;
using System.Collections.Generic;

public class PlayerVortex : MonoBehaviour
{
    [Header("==Vortex Properties==")]
    public float vortexRadius = 2f;
    public int maxAbsorbedProjectiles = 10;
    public float damageMultiplierPerProjectile = 0.2f;
    public float vortexDrainRate = 30f; // Energy drained per second

    [Header("==Visual Vortex Effects==")]
    public GameObject vortexVisualEffect;
    public float rotationSpeed = 180f;

    private Player player;
    private List<Projectile> absorbedProjectiles = new List<Projectile>();
    private int absorbedCount = 0;
    private GameObject activeVortexEffect;
    private CircleCollider2D vortexCollider;

    private void Awake()
    {
        player = GetComponent<Player>();

        // create vortex collider
        vortexCollider = gameObject.AddComponent<CircleCollider2D>();
        vortexCollider.radius = vortexRadius;
        vortexCollider.isTrigger = true;
        vortexCollider.enabled = false; // Initially disabled
    }

    public void ActivateVortex()
    {
        vortexCollider.enabled = true;
        absorbedCount = 0;
        absorbedProjectiles.Clear();

        // create visual effect
        if (vortexVisualEffect != null)
        {
            activeVortexEffect = Instantiate(vortexVisualEffect, transform);
        }

        Debug.Log("Vortex activated");
    }

    public void DeactivateVortex()
    {
        vortexCollider.enabled = false;

        // destroy visual effect
        if (activeVortexEffect != null)
        {
            Destroy(activeVortexEffect);
        }

        Debug.Log("Vortex deactivated");
    }

    void Update()
    {
        // rotate visual effect for vortex
        if (activeVortexEffect != null)
        {
            activeVortexEffect.transform.Rotate(0,0,rotationSpeed * Time.deltaTime);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // when vortex is active
        if (!vortexCollider.enabled) return;

        // check if enemy projectile
        if (collision.gameObject.TryGetComponent<Projectile>(out var projectile))
        {
            // targeting player layer
            if (projectile.attacking_layer == 6 && absorbedCount < maxAbsorbedProjectiles)
            {
                AbsorbProjectile(projectile.gameObject);
            }
        }
    }

    private void AbsorbProjectile(GameObject projectile)
    {
        absorbedCount++;

        // for visual/audio feedback
        if (player.audioManager != null)
        {
            player.audioManager.PlayAudioSource("ShieldStart");
        }

        Destroy(projectile);


        Debug.Log($"Absorbed projectile! Count: {absorbedCount}");
    }

    public float GetDamageMultiplier()
    {
        float multiplier = 1f + (absorbedCount * damageMultiplierPerProjectile);
        return multiplier;
    }

    public int GetAbsorbedCount()
    {
        return absorbedCount;
    }

    public void ResetAbsorbedCount()
    {
        absorbedCount = 0;

        // add UI reset and then visual effects here
    }

    public float GetVortexDrainRate()
    {
        return vortexDrainRate;
    }

}

using System;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.InputSystem;

public class Weapon : MonoBehaviour
{
    public enum FireMode
    {
        FULL_AUTO,
        SEMI_AUTO,
        CHARGE,
        LOCK_ON
    }

    [Header("==Necessary GameObjects==")]
    public GameObject projectile;
    public Sprite weaponSprite;

    [Header("==Weapon Properties==")]
    public int currentAmmo;
    public int maxAmmo;
    public int ammoCost;
    public float fireRate;
    public float reloadTime;
    public int reloadAmount;
    public float reloadSlowDownFactor = 0.75f;
    public bool infiniteAmmo = false;
    public ProjectileData projData;

    // Firemode is used to determine firing behavior
    // Semi auto seems to work when firerate is = 0 but that leads to division by zero which is an underfined behavior
    public FireMode fireMode;

    // private vars
    [NonSerialized] public Entity owner;
    [NonSerialized] public float fireTimer = 0f;
    [NonSerialized] public List<Transform> targets = new List<Transform> { };
    private bool isCharging = false;
    private bool isCharged = false;
    private string[] layerMask = { "Enemy" };

    [SerializeField] private int maxLockOnShots = 3;
    [SerializeField] private float lockOnCooldown = 1f;

    void Start()
    {
        // ammo underflow/overflow check
        if (currentAmmo < 0) currentAmmo = 0;
        if (currentAmmo > maxAmmo) currentAmmo = maxAmmo;
    }

    public void Shoot(InputActionReference fireAction, Transform firePoint, int collision_layer, string fireSFX = "")
    {
        if (currentAmmo - ammoCost < 0) return;

        if (fireAction != null) // player shooting
        {
            if (fireMode == FireMode.FULL_AUTO)
            {
                fullAutoShot(fireAction, firePoint, collision_layer, fireSFX);
            }
            else if (fireMode == FireMode.SEMI_AUTO)
            {
                semiAutoShot(fireAction, firePoint, collision_layer);
            }
            else if (fireMode == FireMode.CHARGE)
            {
                chargeShot(fireAction, firePoint, collision_layer);
            }
            else if (fireMode == FireMode.LOCK_ON)
            {
                lockOnShot(fireAction, firePoint, collision_layer);
            }
        }
        else // other entities
        {
            switch (fireMode)
            {
                case FireMode.FULL_AUTO:
                    fireTimer -= Time.deltaTime;
                    if (fireTimer <= 0f && infiniteAmmo)
                    {
                        fireTimer += 1f / fireRate;
                        ShootProjectile(firePoint, collision_layer);
                        owner.audioManager.PlayAudioSource("Shoot");
                    }

                    break;

                case FireMode.SEMI_AUTO:
                    ShootProjectile(firePoint, collision_layer);
                    owner.audioManager.PlayAudioSource("Shoot");
                    break;

                case FireMode.CHARGE:
                    if (!isCharging)
                    {
                        isCharging = true;
                        fireTimer = 0f;
                        owner.audioManager.PlayAudioSource("ShootStart");
                    }

                    if (isCharging) fireTimer += Time.deltaTime;

                    if (fireTimer >= projData.timeToSpawn)
                    {
                        ShootProjectile(firePoint, collision_layer);
                        owner.audioManager.PlayAudioSource("Shoot");
                        isCharging = false;
                        fireTimer = 0f;
                    }

                    break;

                case FireMode.LOCK_ON:
                    fireTimer -= Time.deltaTime;

                    if (fireTimer <= 0f)
                    {
                        // enemy fires at player
                        Transform target = LevelManager.player.gameObject.transform;

                        int shotsToFire = Mathf.Min(maxLockOnShots, 1); // one target, 2 missile for now
                        for (int i = 0; i < shotsToFire; i++)
                        {
                            // put audio manager here

                            Missile missile = (Missile)ShootProjectile(firePoint, collision_layer);
                            missile.target = LevelManager.player.gameObject.transform;
                        }
                        fireTimer = lockOnCooldown; // reset cooldown
                    }
                    break;
            }
        }
    }

    private void fullAutoShot(InputActionReference fireAction, Transform firePoint, int collision_layer , string fireSFX = "")
    {
        if (fireAction.action.IsPressed())
        {
            fireTimer -= Time.deltaTime;
            if (fireTimer <= 0f)
            {
                fireTimer += 1f / fireRate;
                ShootProjectile(firePoint, collision_layer);

                float startTime = 0.0f;
                if (fireSFX.Equals("FlamethrowerFire"))
                    startTime = 0.85f;

                owner.audioManager.PlayAudioSource(fireSFX, startTime);
            }
        }
        
        if (fireAction.action.WasReleasedThisFrame() && fireSFX.Equals("FlamethrowerFire"))
        {
            owner.audioManager.PauseAudioSource(fireSFX);
            float startTime = 5.2f;
            owner.audioManager.PlayAudioSource(fireSFX, startTime);
        }
    }

    private void semiAutoShot(InputActionReference fireAction, Transform firePoint, int collision_layer)
    {
        if (fireAction.action.WasPressedThisFrame())
        {
            ShootProjectile(firePoint, collision_layer);
            //owner.audioManager.PlayAudioSource("");
        }
    }

    private void chargeShot(InputActionReference fireAction, Transform firePoint, int collision_layer)
    {
        // Start charging when the player holds the button
        if (fireAction.action.IsPressed())
        {
            if (!isCharging)
            {
                isCharging = true;
                isCharged = false;
                fireTimer = 0f;
                //Debug.Log("Started charging");
            }

            //Debug.Log($"Charging... {fireTimer:F2}s");
            fireTimer += Time.deltaTime; // Increment charge timer while holding
            
            if (fireTimer > projData.timeToSpawn && !isCharged)
            {
                owner.audioManager.PlayAudioSource("RailgunCharged");
                isCharged = true;
            }
        }

        // Fire when player releases the button
        if (isCharged && fireAction.action.WasReleasedThisFrame())
        {
            //Debug.Log($"Released at {fireTimer:F2}s");
            ShootProjectile(firePoint, collision_layer);
            owner.audioManager.PlayAudioSource("RailgunFire");
            isCharging = false; // Reset for next charge

            fireTimer = 0f;
        }        
    }

    private void lockOnShot(InputActionReference fireAction, Transform firePoint, int collision_layer)
    {
        if (fireAction.action.IsPressed())
        {
            // get targets hit by mouse
            Vector2 mousePos = (owner as Player).cam.ScreenToWorldPoint(Mouse.current.position.ReadValue());

            Collider2D selected = Physics2D.OverlapPoint(mousePos, LayerMask.GetMask(layerMask));

            //print(selected);
            if (selected != null && !targets.Contains(selected.gameObject.transform))
            {
                targets.Add(selected.gameObject.transform);
            }
        }
        if (fireAction.action.WasReleasedThisFrame())
        {
            // fire a projectile for each target
            foreach (Transform target in targets)
            {
                if (target == null) continue;

                owner.audioManager.PlayAudioSource("MissileFire");

                Missile missile = (Missile)ShootProjectile(firePoint, collision_layer, false);
                missile.target = target;
                missile.gameObject.SetActive(true);
            }
            targets.Clear();
        }
    }

    public void ReloadWeapon()
    {
        currentAmmo += reloadAmount;
        if (currentAmmo > maxAmmo) currentAmmo = maxAmmo;
        Debug.Log("Weapon reloaded");
        // play SFX/VFX
    }
    
    // Overload function for partil reloads (revoler, missile launcher, etc)
    public void ReloadWeapon(float howLongReloadWasHeld)
    {
        currentAmmo += reloadAmount * (int)howLongReloadWasHeld;
        if (currentAmmo > maxAmmo) currentAmmo = maxAmmo;
        Debug.Log("Weapon reloaded");
        // play SFX/VFX
    }

    public Projectile ShootProjectile(Transform firePoint, int receiving_layer, bool active = true)
    {
        // update ammo
        if (!infiniteAmmo) currentAmmo -= ammoCost;

        // create projectile
        Projectile proj = Instantiate(projectile, firePoint.position, firePoint.rotation, LevelManager.current_level.EntityList.transform).GetComponent<Projectile>();

        proj.projData = Instantiate(projData);
        proj.attacking_layer = receiving_layer;

        // Vortex damage booster when player is active with vortex
        if (owner is Player player)
        { 
            int absorbedCount = player.GetAbsorbedCount();

            if (absorbedCount >= 0 && player.getIsVortexBlocking())
            { 
                // Apply damage multiplier
                float multiplier = player.GetDamageMultiplier();
                int originalDamage = proj.projData.damage;
                int boostedDamage = Mathf.RoundToInt(originalDamage * multiplier);
                proj.projData.damage = boostedDamage;

                Debug.Log($"Vortex boost! Damage: {originalDamage} -> {boostedDamage} (x{multiplier:F2}, absorbed: {absorbedCount}");
            }
        }

        proj.gameObject.SetActive(active);

        return proj;
    }
}
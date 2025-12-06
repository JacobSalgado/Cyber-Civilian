using System;
using System.Collections.Generic;
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
    //public int reloadAmount;
    public float reloadSlowDownFactor = 0.75f;
    public bool infiniteAmmo = false;
    public ProjectileData projData;

    // Firemode is used to determine firing behavior
    // Semi auto seems to work when firerate is = 0 but that leads to division by zero which is an underfined behavior
    public FireMode fireMode;

    // private vars
    [NonSerialized] public Entity owner;
    [NonSerialized] public float fireTimer = 0f;
    private bool isCharging = false;
    private bool isCharged = false;

    void Start()
    {
        // ammo underflow/overflow check
        if (currentAmmo < 0) currentAmmo = 0;
        if (currentAmmo > maxAmmo) currentAmmo = maxAmmo;
    }

    public void Shoot(InputActionReference fireAction, Transform firePoint, int collision_layer, string fireSFX = "")
    {
        if (fireAction != null) // player shooting
        {
            if (fireMode == FireMode.FULL_AUTO)
            {
                PlayerFullAutoShot(fireAction, firePoint, collision_layer, fireSFX);
            }
            else if (fireMode == FireMode.SEMI_AUTO)
            {
                PlayerSemiAutoShot(fireAction, firePoint, collision_layer);
            }
            else if (fireMode == FireMode.CHARGE)
            {
                PlayerChargeShot(fireAction, firePoint, collision_layer);
            }
            else if (fireMode == FireMode.LOCK_ON)
            {
                PlayerLockOnShot(fireAction, firePoint, collision_layer);
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
                    // enemy fires at player
                    Missile missile = (Missile) ShootProjectile(firePoint, collision_layer);
                    missile.target = LevelManager.player.gameObject.transform;
                    missile.gameObject.SetActive(true);
                    owner.audioManager.PlayAudioSource("Shoot");
                    
                    break;
            }
        }
    }

    private void PlayerFullAutoShot(InputActionReference fireAction, Transform firePoint, int collision_layer , string fireSFX = "")
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
            owner.audioManager.PlayAudioSource(fireSFX, 5.2f);
        }
    }

    private void PlayerSemiAutoShot(InputActionReference fireAction, Transform firePoint, int collision_layer)
    {
        fireTimer -= Time.deltaTime;
        if (fireAction.action.WasPressedThisFrame())
        {
            if (fireTimer <= 0f)
            {
                fireTimer += 1f/fireRate;
                ShootProjectile(firePoint, collision_layer);
                //owner.audioManager.PlayAudioSource("");
            }
        }
    }

    private void PlayerChargeShot(InputActionReference fireAction, Transform firePoint, int collision_layer)
    {
        Player player = (Player) owner;

        // Start charging when the player holds the button
        if (fireAction.action.IsPressed())
        {
            if (!isCharging)
            {
                isCharging = true;
                isCharged = false;
                fireTimer = 0f;
                player.chargeMeter.TurnOnMeter(ChargeMeter.MeterType.RAILGUN_CHARGE, 0f, projData.timeToSpawn);
                //Debug.Log("Started charging");
            }

            //Debug.Log($"Charging... {fireTimer:F2}s");
            fireTimer += Time.deltaTime; // Increment charge timer while holding

            if (fireTimer > projData.timeToSpawn && !isCharged)
            {
                player.chargeMeter.TurnOffMeter();
                owner.audioManager.PlayAudioSource("RailgunCharged");
                isCharged = true;
            }
        }

        // Fire when player releases the button
        if (fireAction.action.WasReleasedThisFrame())
        {
            if (isCharged){
                //Debug.Log($"Released at {fireTimer:F2}s");
                ShootProjectile(firePoint, collision_layer);
                owner.audioManager.PlayAudioSource("RailgunFire");
            }
            player.chargeMeter.TurnOffMeter();
            isCharging = false; // Reset for next charge

            fireTimer = 0f;
        }        
    }

    private void PlayerLockOnShot(InputActionReference fireAction, Transform firePoint, int collision_layer)
    {
        Player player = owner as Player;
        if (player.missileRadius.onCooldown) return;

        if (fireAction.action.IsPressed()){
            player.missileRadius.ToggleRadius(true);
        }
        if (fireAction.action.WasReleasedThisFrame())
        {
            // fire a projectile for each target
            foreach (Transform target in player.missileRadius.targetList)
            {
                if (target == null) continue;

                owner.audioManager.PlayAudioSource("MissileFire");

                Missile missile = (Missile)ShootProjectile(firePoint, collision_layer, false);
                missile.target = target;
                missile.gameObject.SetActive(true);
            }

            player.missileRadius.ToggleRadius(false);
            player.missileRadius.cooldownTime = fireRate;
        }
    }

    public void ReloadWeapon()
    {
        currentAmmo = maxAmmo;
        // play SFX/VFX
    }
    
    // Overload function for partil reloads (revoler, missile launcher, etc)
    public void ReloadWeapon(float howLongReloadWasHeld)
    {
        currentAmmo = maxAmmo;
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
        if (owner is Player player && player.bonusDamageSet)
        { 
            // Apply damage multiplier
            int originalDamage = proj.projData.damage;
            int boostedDamage = Mathf.CeilToInt(originalDamage * player.bonusDamageMultiplier);
            proj.projData.damage = boostedDamage;

            Debug.Log($"Vortex boost! Damage: {originalDamage} -> {boostedDamage} (x{player.bonusDamageMultiplier:F2}");

            proj.projData.damage = boostedDamage;
        }
        else if (owner is CyberBoss cyberBoss && cyberBoss.inRageMode)
        {
            int originalDamage = proj.projData.damage;
            int boostedDamage = Mathf.CeilToInt(originalDamage * cyberBoss.rageDamageBoost);

            Debug.Log($"Vortex boost! Damage: {originalDamage} -> {boostedDamage} (x{cyberBoss.rageDamageBoost:F2}");

            proj.projData.damage = boostedDamage;
        }

        proj.gameObject.SetActive(active);

        return proj;
    }
}
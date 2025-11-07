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

    void Start()
    {
        // ammo underflow/overflow check
        if (currentAmmo < 0) currentAmmo = 0;
        if (currentAmmo > maxAmmo) currentAmmo = maxAmmo;
    }

    public void Shoot(InputActionReference fireAction, Transform firePoint, int collision_layer)
    {
        if (currentAmmo - ammoCost < 0) return;

        if (fireAction != null) // player shooting
        {
            if (fireMode == FireMode.FULL_AUTO && fireAction.action.IsPressed())
            {
                fireTimer -= Time.deltaTime;
                // Debug.Log(fireTimer); // testing how firetimer works
                if (fireTimer <= 0f)
                {
                    fireTimer += 1f / fireRate;
                    ShootProjectile(firePoint, collision_layer);
                    owner.audioManager.PlayAudioSource("PeaShooterFire");
                }
            }
            else if (fireMode == FireMode.SEMI_AUTO && fireAction.action.WasPressedThisFrame())
            {
                ShootProjectile(firePoint, collision_layer);
                //owner.audioManager.PlayAudioSource("");
            }
            else if (fireMode == FireMode.CHARGE)
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
            else if (fireMode == FireMode.LOCK_ON)
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
            }
        }
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

            if (absorbedCount > 0)
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
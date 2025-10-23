using System;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class Weapon : MonoBehaviour
{
    public enum FireMode
    {
        FULL_AUTO,
        SEMI_AUTO,
        CHARGE
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
    // 0 = full auto, 1 = semi auto
    // Semi auto seems to work when firerate is = 0 but that leads to division by zero which is an underfined behavior
    public FireMode fireMode;

    // private vars
    [NonSerialized] public float fireTimer = 0f;
    private bool isCharging = false;
    

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
                }
            }
            else if (fireMode == FireMode.SEMI_AUTO && fireAction.action.WasPressedThisFrame())
            {
                ShootProjectile(firePoint, collision_layer);
            }
            else if (fireMode == FireMode.CHARGE)
            {
                // Start charging when the player holds the button
                if (fireAction.action.IsPressed())
                {
                    if (!isCharging)
                    {
                        isCharging = true;
                        fireTimer = 0f;
                        //Debug.Log("Started charging");
                    }

                    // Increment charge timer while holding
                    fireTimer += Time.deltaTime;
                    //Debug.Log($"Charging... {fireTimer:F2}s");
                }

                // Fire when player releases the button
                if (isCharging && fireAction.action.WasReleasedThisFrame())
                {
                    if (fireTimer >= projData.timeToSpawn)
                    {
                        //Debug.Log($"Released at {fireTimer:F2}s");
                        ShootProjectile(firePoint, collision_layer);
                        isCharging = false; // Reset for next charge
                    }
                    
                    fireTimer = 0f;
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
                    }

                    break;

                case FireMode.SEMI_AUTO:
                    ShootProjectile(firePoint, collision_layer);
                    break;

                case FireMode.CHARGE:
                    if (!isCharging)
                    {
                        isCharging = true;
                        fireTimer = 0f;
                    }

                    if (isCharging) fireTimer += Time.deltaTime;

                    if (fireTimer >= projData.timeToSpawn)
                    {
                        ShootProjectile(firePoint, collision_layer);
                        isCharging = false;
                        fireTimer = 0f;
                    }

                    break;
            }
        }
    }

    public void ShootProjectile(Transform firePoint, int receiving_layer)
    {
        // update ammo
        if (!infiniteAmmo) currentAmmo -= ammoCost;

        // create projectile
        Projectile proj = Instantiate(projectile, firePoint.position, firePoint.rotation, LevelManager.current_level.EntityList.transform).GetComponent<Projectile>();

        proj.projData = Instantiate(projData);
        proj.audioManager.InitializeAudioDictionary(proj.projData.SFXNames, proj.projData.SFX);
        proj.attacking_layer = receiving_layer;
    }
}
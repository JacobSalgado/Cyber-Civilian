using UnityEngine;
using UnityEngine.InputSystem;

public class Weapon : MonoBehaviour
{
    public enum FireMode
    {
        FULL_AUTO,
        SEMI_AUTO
    }

    [Header("==Necessary GameObjects==")]
    public GameObject projectile;
    public Sprite weaponSprite;

    [Header("==Weapon Properties==")]
    public int damage;
    public float projectileForce;
    public int currentAmmo;
    public int maxAmmo;
    public int ammoCost;
    public float fireRate;
    public bool infiniteAmmo = false;
    private float fireTimer;

    // Firemode is used to determine firing behavior
    // 0 = full auto, 1 = semi auto
    // Semi auto seems to work when firerate is = 0 but that leads to division by zero which is an underfined behavior
    public FireMode fireMode;

    void Start()
    {
        // ammo underflow/overflow check
        if (currentAmmo < 0) currentAmmo = 0;
        if (currentAmmo > maxAmmo) currentAmmo = maxAmmo;
    }

    public void Shoot(InputActionReference fireAction, Transform firePoint, int collision_layer)
    {
        if (fireAction != null) // player shooting
        {
            if (fireMode == FireMode.FULL_AUTO && fireAction.action.IsPressed())
            {
                fireTimer -= Time.deltaTime;
                if (fireTimer <= 0f && currentAmmo - ammoCost >= 0)
                {
                    if (!infiniteAmmo) currentAmmo -= ammoCost;
                    fireTimer += 1f / fireRate;
                    ShootProjectile(firePoint, collision_layer);
                }
            }
            else if (fireMode == FireMode.SEMI_AUTO && fireAction.action.WasPressedThisFrame() && currentAmmo - ammoCost >= 0)
            {
                if (!infiniteAmmo) currentAmmo -= ammoCost;
                ShootProjectile(firePoint, collision_layer);
            }
            else
            {
                fireTimer = 0f;
            }
        }
        else // other entities
        {
            switch (fireMode)
            {
                case FireMode.FULL_AUTO:
                    fireTimer -= Time.deltaTime;
                    if (fireTimer <= 0f && (infiniteAmmo || currentAmmo - ammoCost >= 0))
                    {
                        if (!infiniteAmmo) currentAmmo -= ammoCost;
                        fireTimer += 1f / fireRate;
                        ShootProjectile(firePoint, collision_layer);
                    }

                    break;

                case FireMode.SEMI_AUTO:
                    if (currentAmmo - ammoCost >= 0)
                    {
                        if (!infiniteAmmo) currentAmmo -= ammoCost;
                        ShootProjectile(firePoint, collision_layer);
                    }
                    break;

                default:
                    fireTimer = 0f;
                    break;
            }
        }
    }

    private void ShootProjectile(Transform firePoint, int collision_layer)
    {
        // create projectile
        string[] ignored_layers = { "Enemy Attacks", "Player Attacks" };
        LayerMask layer = LayerMask.GetMask(ignored_layers);

        Projectile proj  = Instantiate(projectile, firePoint.position, firePoint.rotation).GetComponent<Projectile>();

        proj.damage = damage;
        proj.force = projectileForce;
        proj.gameObject.layer = collision_layer;
        proj.rigidBody.excludeLayers = layer;        
    }
}
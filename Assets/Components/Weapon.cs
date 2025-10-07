using UnityEngine;
using UnityEngine.InputSystem;

public class Weapon : MonoBehaviour
{
    public int currentAmmo;
    public int maxAmmo;
    public GameObject proj;
    public Sprite weaponSprite;
    public float fireTimer;
    public float fireRate;

    // Firemode is used to determine firing behavior
    // 0 = full auto, 1 = semi auto
    // Semi auto seems to work when firerate is = 0 but that leads to division by zero which is an underfined behavior
    public int fireMode;

    public void ShootWeapon(InputActionReference fireAction, Transform firePoint, int collision_layer)
    {
        if (fireMode == 0 && fireAction.action.IsPressed())
        {
            fireTimer -= Time.deltaTime;
            if (fireTimer <= 0f)
            {
                Shoot(firePoint, collision_layer);

                fireTimer += 1f / fireRate;
            }
        }
        else if (fireMode == 1 && fireAction.action.WasPressedThisFrame())
        {
            Shoot(firePoint, collision_layer);
        }
        else
        {
            fireTimer = 0f;
        }
    }

    private void Shoot(Transform firePoint, int collision_layer)
    {
        // create projectile
        string[] ignored_layers = { "Enemy Attacks", "Player Attacks" };
        LayerMask layer = LayerMask.GetMask(ignored_layers);

        Projectile projectile = Instantiate(proj, firePoint.position, firePoint.rotation).GetComponent<Projectile>();
        projectile.gameObject.layer = collision_layer;
        projectile.rigidBody.excludeLayers = layer;        
    }
}
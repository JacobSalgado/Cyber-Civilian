using UnityEngine;
using UnityEngine.InputSystem;

public class Weapon : MonoBehaviour
{
    public int currentAmmo;
    public int maxAmmo;
    public GameObject proj;

    public float fireTimer;
    public float fireRate;

    public void ShootWeapon(InputActionReference fireAction, Transform firePoint, int collision_layer)
    {
        if (fireAction.action.IsPressed())
        {
            fireTimer -= Time.deltaTime;
            if (fireTimer <= 0f)
            {
                // create projectile
                string[] ignored_layers = { "Enemy Attacks", "Player Attacks" };
                LayerMask layer = LayerMask.GetMask(ignored_layers);

                Projectile projectile = Instantiate(proj, firePoint.position, firePoint.rotation).GetComponent<Projectile>();
                projectile.gameObject.layer = collision_layer;
                projectile.rigidBody.excludeLayers = layer;

                fireTimer += 1f / fireRate;
            }
        }
        else
        {
            fireTimer = 0f;
        }
    }
}
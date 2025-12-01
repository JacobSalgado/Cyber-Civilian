using System;
using UnityEngine;

public abstract class Enemy : Entity
{
    public enum EnemyTypes
    {
        SHARK,
        CRAB,
        MANTIS,
        TRAPPER,
        HOMING,
        CYBERBOSS,
    }

    [Header("==Enemy GameObjects==")]
    public Transform firePoint;
    public GameObject weapon;
    public GameObject canvas;
    public SpriteRenderer missileTargetedSprite;

    // Non-Serialized Vars
    [NonSerialized] public Transform target; // following the player
    [NonSerialized] public EnemyTypes type;

    public override void Start()
    {
        base.Start();
        target = LevelManager.player.transform;

        if (weapon != null) {
            weapon = Instantiate(weapon, transform);
            weapon.GetComponent<Weapon>().owner = this;
        }
        UpdateHealthBar();
    }

    /// <summary>
    /// Calculates distance to target
    /// </summary>
    /// <returns>
    /// If target is not null, returns distance to target as float
    /// If target is null, returns -1
    /// </returns>
    public float GetDistanceToTarget()
    {
        if (target != null)
        {
            float distance = Vector2.Distance(transform.position, target.position);
            return distance;
        }

        return -1f;
    }

    public override void FixedUpdate()
    {
        base.FixedUpdate();

        // undo healthBar rotation
        canvas.transform.rotation = Quaternion.identity;

        // undo missile targeted sprite rotation
        missileTargetedSprite.gameObject.transform.localRotation = Quaternion.identity;
    }

    public override void EntityDie()
    {
        LevelManager.enemyKilledCounter += 1;
        LevelManager.player.SetHealth(LevelManager.player.playerData.currentHealth + 100); // TODO: do something cooler
        GameObject deathEffect = Instantiate(deathEffectPrefab, gameObject.transform.position, gameObject.transform.rotation, LevelManager.current_level.EntityList.transform);

        deathEffect.GetComponent<ParticleSystem>().Play();
        Destroy(gameObject);
    }
}

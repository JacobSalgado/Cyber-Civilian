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

    // Non-Serialized Vars
    [NonSerialized] public Transform target; // following the player
    [NonSerialized] public EnemyTypes type;

    public override void Start()
    {
        base.Start();
        target = LevelManager.player.transform;

        weapon = Instantiate(weapon, transform);
        weapon.GetComponent<Weapon>().owner = this;
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
    }

    public override void EntityDie()
    {
        LevelManager.enemyKilledCounter += 1;
        LevelManager.player.SetHealth(LevelManager.player.playerData.currentHealth + 70); // TODO: do something cooler
        Destroy(gameObject);
    }
}

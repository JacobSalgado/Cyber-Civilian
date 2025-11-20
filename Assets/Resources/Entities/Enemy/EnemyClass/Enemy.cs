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

    [NonSerialized] public Vector2 pushedVelocity;
    [NonSerialized] public Vector2 moveVelocity = Vector2.zero;

    // Non-Serialized Vars
    [NonSerialized] public Transform target; // following the player
    [NonSerialized] public EnemyTypes type;
    [NonSerialized] public bool pushed = false;
    const float PUSHED_TIME = 0.5f;
    float pushTimer = 0f;

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

        // check push timing
        if (pushed)
        {
            pushTimer += Time.deltaTime;

            // decrease push velocity
            rigidBody.linearVelocity = pushedVelocity;
            pushedVelocity *= 0.85f;

            if (pushTimer > PUSHED_TIME)
            {
                pushTimer = 0f;
                pushed = false;
            }
        }
        else rigidBody.linearVelocity = moveVelocity;       
    }

    public override void EntityDie()
    {
        LevelManager.enemyKilledCounter += 1;
        Destroy(gameObject);
    }
}

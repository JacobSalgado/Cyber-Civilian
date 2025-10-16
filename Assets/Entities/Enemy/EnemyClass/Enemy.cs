using System;
using UnityEngine;

public abstract class Enemy : Entity
{
    [Header("==Enemy GameObjects==")]
    public Transform firePoint;
    public GameObject weapon;

    public Player player;

    // Non-Serialized Vars
    [NonSerialized] public Transform target; // following the player
    //[NonSerialized] public bool isAggro;

    /// <summary>
    /// Calculates distance to target
    /// </summary>
    /// <returns>
    /// If target is not null, returns distance to target as float
    /// If target is null, returns -1
    /// </returns>

    public override void Start()
    {
        base.Start();
        target = LevelManager.player.transform;
        weapon = Instantiate(weapon);
        weapon.transform.SetParent(transform, false);
    }

    public float GetDistanceToTarget()
    {
        if (target != null)
        {
            float distance = Vector2.Distance(transform.position, target.position);
            return distance;
        }

        return -1f;
    }

    public override void EntityDie()
    {
        LevelManager.enemyKilledCounter += 1;
        Destroy(gameObject);
    }

    // doing it in here since enemies get instantiated
    /*public void PlayerShield()
    {
        Vector2 playerForward = player.firePoint.right;
        Vector2 toPlayer = (player.transform.position - transform.position).normalized;

        float dot = Vector2.Dot(playerForward, toPlayer);
        if (dot > Mathf.Cos(45f * Mathf.Deg2Rad) && player.getIsBlocking())
        {
            player.invincibility = true;
        }
        else
        {
            player.invincibility = false;
        }
    }*/
}

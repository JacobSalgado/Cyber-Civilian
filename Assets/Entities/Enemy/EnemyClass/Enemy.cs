using System;
using UnityEngine;

public abstract class Enemy : Entity
{
    [Header("==Enemy GameObjects==")]
    public Transform firePoint;
    public GameObject weapon;

    // Non-Serialized Vars
    //[NonSerialized] public bool isAggro;
    public Transform target; // following the player

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
}

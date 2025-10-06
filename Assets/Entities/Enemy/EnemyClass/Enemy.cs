using UnityEngine;

public abstract class Enemy : Entity
{
    [Header("Enemy Properties")]
    public Transform target; // following the player
    public GameObject proj;
    //[NonSerialized] public bool isAggro;

    // TODO: add weapon variable


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
}

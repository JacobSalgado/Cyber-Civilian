using UnityEngine;

public abstract class Enemy : Entity
{
    [Header("==Enemy GameObjects==")]
    public Transform target; // following the player
    public GameObject weapon;
    //[NonSerialized] public bool isAggro;

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

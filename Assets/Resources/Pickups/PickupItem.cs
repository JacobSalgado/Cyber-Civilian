using System;
using UnityEngine;

public class PickupItem : MonoBehaviour
{
    // Object lifecycle variables
    [SerializeField] private float lifeTime = 100f;
    [NonSerialized] private float lifeTimer = 0f;

    public virtual void FixedUpdate()
    {
        lifeTimer += Time.deltaTime;
        if (lifeTimer >= lifeTime)
        {
            Destroy(gameObject);
        }
    }
}

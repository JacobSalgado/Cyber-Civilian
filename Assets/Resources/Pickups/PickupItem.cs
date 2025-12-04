using System;
using UnityEngine;

public class PickupItem : MonoBehaviour
{
    // Object lifecycle variables
    [SerializeField] private float lifeTime = 100f;
    [NonSerialized] private float lifeTimer = 0f;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public virtual void FixedUpdate()
    {
        lifeTimer += Time.deltaTime;
        if (lifeTimer >= lifeTime)
        {
            Destroy(gameObject);
        }
    }
}

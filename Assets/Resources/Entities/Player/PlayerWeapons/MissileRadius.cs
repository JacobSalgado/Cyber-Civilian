using System;
using System.Collections.Generic;
using UnityEngine;

public class MissileRadius : MonoBehaviour
{
    [SerializeField] private CircleCollider2D aoeCollider;
    [SerializeField] private SpriteRenderer aoeVisual;

    [NonSerialized] public List<Transform> targetList = new() { };
    [NonSerialized] public float cooldownTime = 0f; // NOTE: set by missile launcher's fire rate
    [NonSerialized] public bool onCooldown = false;
    private float timer = 0f;

    void Start()
    {
        aoeCollider.enabled = false;
        aoeVisual.enabled = false;
    }

    void FixedUpdate()
    {
        gameObject.transform.SetLocalPositionAndRotation(Vector2.zero, Quaternion.identity);

        if (onCooldown)
        {
            timer += Time.deltaTime;
            if (timer > cooldownTime) onCooldown = false;
        }
    }

    public void ToggleRadius(bool toggle)
    {
        if (onCooldown) return;

        aoeCollider.enabled = toggle;
        aoeVisual.enabled = toggle;
        if (!toggle) 
        {
            ClearTargetList();
            timer = 0f;
            onCooldown = true;
        }
    }

    public void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.gameObject.TryGetComponent<Enemy>(out var enemy))
        {
            targetList.Add(enemy.gameObject.transform);
            enemy.missileTargetedSprite.enabled = true;
        }
    }

    public void OnTriggerExit2D(Collider2D collider)
     {
         if (collider.gameObject.TryGetComponent<Enemy>(out var enemy))
         {
             targetList.Remove(enemy.gameObject.transform);
             enemy.missileTargetedSprite.enabled = false;
        }
    }

    private void ClearTargetList()
    {
        foreach (Transform target in targetList)
        {
            target.gameObject.GetComponent<Enemy>().missileTargetedSprite.enabled = false;
        }
        targetList.Clear();
    }
}

using System;
using System.Collections.Generic;
using UnityEngine;

public class MissileRadius : MonoBehaviour
{
    [NonSerialized] public List<Transform> targetList = new() { };
    [SerializeField] private CircleCollider2D aoeCollider;
    [SerializeField] private SpriteRenderer aoeVisual;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        aoeCollider.enabled = false;
        aoeVisual.enabled = false;
    }

    void FixedUpdate()
    {
        gameObject.transform.SetLocalPositionAndRotation(Vector2.zero, Quaternion.identity);
    }

    public void ToggleRadius(bool toggle)
    {
        aoeCollider.enabled = toggle;
        aoeVisual.enabled = toggle;
        if (toggle == false) ClearTargetList();
    }

    public void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.gameObject.TryGetComponent<Enemy>(out var enemy))
        {
            targetList.Add(enemy.gameObject.transform);
            enemy.missileTargetedSprite.enabled = true;
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

    public void OnTriggerExit2D(Collider2D collider)
     {
         if (collider.gameObject.TryGetComponent<Enemy>(out var enemy))
         {
             targetList.Remove(enemy.gameObject.transform);
             enemy.missileTargetedSprite.enabled = false;
        }
    }
}

using System;
using UnityEngine;
public class TrapField : Entity
{
    public SpriteRenderer spriteRenderer;
    public Collider2D trapCollider;
    public float shockDuration = 0f;
    public float slowDownStrength = 0f;
    public float fadeawayTime = 1f;
    public float lifeTime = 5f;
    public override void InitializeStates()
    {
        AddState("Active", new TrapFieldActive(this));
        AddState("FadeAway", new TrapFieldFadeAway(this));

        ChangeState("Active");
    }

    public override void Start()
    {
        base.Start();
        InitializeStates();
    }
    public override void EntityDie()
    {
        Destroy(gameObject);
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("Collided");
        Debug.Log(collision.gameObject.name);
        if (collision.gameObject.TryGetComponent<Player>(out var player))
        {
            if (!player.invincibility)
            {
                player.ApplyShockEffect(shockDuration, slowDownStrength);
            }
        }
    }
}
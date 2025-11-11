using UnityEngine;
using System.Collections.Generic;

public class TrapFieldActive : State
{
    readonly TrapField trapField;
    private float timer;

    public TrapFieldActive(Entity new_entity) : base(new_entity)
    {
        trapField = (TrapField)new_entity;
    }

    public override void EnterState(Dictionary<string, object> args = null)
    {
        trapField.rigidBody.linearVelocity = Vector2.zero;
        timer = 0f;
    }

    public override void UpdateState()
    {
        timer += Time.deltaTime;
        if (timer >= trapField.lifeTime)
        {
            if (trapField.fadeawayTime > 0)
                trapField.ChangeState("FadeAway");
            else trapField.EntityDie();
        }
    }

    public override void ExitState(Dictionary<string, object> args = null)
    {

    }
    
    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.TryGetComponent<Entity>(out var entity))
        {
            if (!entity.invincibility)
            {
                entity.ApplyShockEffect(trapField.shockDuration, trapField.slowDownStrength);
            }
        }
    }
}

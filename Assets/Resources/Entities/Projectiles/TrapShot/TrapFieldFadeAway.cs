using UnityEngine;
using System.Collections.Generic;

public class TrapFieldFadeAway : State
{
    readonly TrapField trapField;
    private float timer;

    public TrapFieldFadeAway(Entity new_entity) : base(new_entity)
    {
        trapField = (TrapField)new_entity;
    }

    public override void EnterState(Dictionary<string, object> args = null)
    {
        timer = 0;
        trapField.trapCollider.enabled = false; // turn off collision
    }

    public override void UpdateState()
    {
        timer += Time.deltaTime;

        if (timer >= trapField.fadeawayTime)
        {
            trapField.EntityDie();
            return;
        }

        Color currentColor = trapField.spriteRenderer.color;
        currentColor.a -= Time.deltaTime / trapField.fadeawayTime;
        trapField.spriteRenderer.color = currentColor;
    }
}
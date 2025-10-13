using UnityEngine;
using System.Collections.Generic;

public class RailshotFadeAway : State
{
    readonly Railshot railshot;
    private float timer;

    public RailshotFadeAway(Entity new_entity) : base(new_entity)
    {
        railshot = (Railshot)new_entity;
    }

    public override void EnterState(Dictionary<string, object> args = null)
    {
        timer = 0;
        railshot.attacking_layer = 0; // turn off collision
    }

    public override void UpdateState()
    {
        timer += Time.deltaTime;

        if (timer >= railshot.projData.fadeawayTime)
        {
            railshot.EntityDie();
            return;
        }

        Color currentColor = railshot.spriteRenderer.color;
        currentColor.a -= Time.deltaTime / railshot.projData.fadeawayTime;
        railshot.spriteRenderer.color = currentColor;
    }
}
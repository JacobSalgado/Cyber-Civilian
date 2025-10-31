using UnityEngine;
using System.Collections.Generic;

public class RailshotActive : State
{
    readonly Railshot railshot;
    private float timer;

    public RailshotActive(Entity new_entity) : base(new_entity)
    {
        railshot = (Railshot)new_entity;
    }

    public override void EnterState(Dictionary<string, object> args = null)
    {
        railshot.rigidBody.linearVelocity = Vector2.zero;
        timer = 0f;
    }

    public override void UpdateState()
    {
        timer += Time.deltaTime;
        if (timer >= railshot.projData.lifeTime)
        {
            if (railshot.projData.fadeawayTime > 0)
                railshot.ChangeState("FadeAway");
            else railshot.EntityDie();
        }
    }

    public override void ExitState(Dictionary<string, object> args = null)
    {
        
    }
}

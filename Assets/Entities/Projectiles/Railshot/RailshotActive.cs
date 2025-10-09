using System.Collections.Generic;
using UnityEngine;

public class RailshotActive : State
{
    readonly Railshot railshot;

    private float chargeTime = 0.5f;
    private float lifeTime = 5f;
    private float timer = 0f;

    private bool hasFired = false;

    public RailshotActive(Entity new_entity) : base(new_entity)
    {
        railshot = (Railshot)new_entity;
    }

    public override void EnterState(Dictionary<string, object> args = null)
    {
        railshot.rigidBody.linearVelocity = Vector2.zero;
    }

    public override void UpdateState()
    {
        timer += Time.deltaTime;

        if (!hasFired && timer >= chargeTime)
        {
            hasFired = true;
        }

        if (timer >= chargeTime + lifeTime)
        {
            GameObject.Destroy(railshot.gameObject);
        }
    }
}

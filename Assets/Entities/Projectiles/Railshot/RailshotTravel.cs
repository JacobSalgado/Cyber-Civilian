using System.Collections.Generic;
using UnityEngine;

public class RailshotTravel : State
{
    readonly Railshot railshot;

    private float lifeTime = 1f;
    private float timer = 0f;
  
    public RailshotTravel(Entity new_entity) : base(new_entity)
    {
        railshot = (Railshot)new_entity; 
    }

    public override void EnterState(Dictionary<string, object> args = null)
    {
        // rkeep STILL
        railshot.rigidBody.linearVelocity = Vector2.zero;

        //  forward force
        // railshot.rigidBody.AddForce(railshot.transform.right * -1f * railshot.force, ForceMode2D.Impulse);
    }

    public override void UpdateState()
    {
        timer += Time.deltaTime;

        if (timer >= lifeTime)
        {
            GameObject.Destroy(railshot.gameObject);
        }
    }
}

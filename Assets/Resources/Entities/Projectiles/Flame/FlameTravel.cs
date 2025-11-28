using System.Collections.Generic;
using UnityEngine;

public class FlameTravel : State
{
    readonly Flame flame;

    Vector2 direction;
    float angleChange = 0f;
    float lifeTimer = 0f;

    public FlameTravel(Entity new_entity) : base(new_entity)
    {
        flame = (Flame) new_entity;
    }

    public override void EnterState(Dictionary<string, object> args = null)
    {
        direction = flame.transform.right;
        angleChange = Random.Range(-30f, 30f);
        Quaternion rotation = Quaternion.AngleAxis(angleChange, Vector3.forward);

        direction = rotation * direction;
    }

    public override void UpdateState()
    {
        lifeTimer += Time.deltaTime;

        if (lifeTimer > flame.projData.fadeawayTime)
        {
            Color currentColor = flame.spriteRenderer.color;
            currentColor.a -= Time.deltaTime / flame.projData.fadeawayTime;
            flame.spriteRenderer.color = currentColor;
        }

        if (lifeTimer > flame.projData.lifeTime)
            flame.EntityDie();

        flame.moveVelocity = direction * -flame.projData.moveSpeed;
    }
}

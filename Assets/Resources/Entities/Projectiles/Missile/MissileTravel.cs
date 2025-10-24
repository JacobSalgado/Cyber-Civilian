using UnityEngine;

public class MissileTravel: State
{
    readonly Missile missile;

    private float lifeTime;
    private Rigidbody2D rb;

    public MissileTravel(Entity new_entity) : base(new_entity)
    {
        missile = (Missile)new_entity;

        lifeTime = missile.projData.lifeTime;
        rb = missile.GetComponent<Rigidbody2D>();
    }

    public override void UpdateState()
    {
        lifeTime -= Time.deltaTime; // decrease lifetime

        if (lifeTime <= 0f)
        {
            missile.EntityDie();
            return;
        }

        // Add how the missile logic will work

        if (missile.projData.homing)
        {
            Vector2 direction = ((Vector2)missile.target.position - rb.position).normalized;
            float rotateAmount = Vector3.Cross(direction, missile.transform.right).z;
            rb.angularVelocity = -rotateAmount * missile.projData.rotateSpeed;

            // Adjust facing direction based on tracking strength
            Vector2 newDirection = Vector2.Lerp(missile.transform.right, direction, missile.projData.trackingStrength * Time.deltaTime).normalized;
            missile.transform.position = newDirection;
        }

        // move forward
        rb.linearVelocity = missile.transform.right * missile.projData.moveSpeed;
    }
}
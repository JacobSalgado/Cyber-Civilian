using UnityEngine;
using System.Collections.Generic;
using Unity.VisualScripting.FullSerializer;

public class CyberBossStompAttack: State
{
    readonly CyberBoss cyberBoss;

    private const float TTL = 0.3f;
    private float timer = 0f;

    [SerializeField] CircleCollider2D aoeCollider;
    [SerializeField] private float shockwaveSpeed = 10f;
    [SerializeField] private float maxShockwaveRadius = 20f;
    [SerializeField] private float pushForce = 15f;

    private bool isShockwaveActive = false;
    private float currentRadius = 0f;
    // hashset if we want to make it so the aoe affects other enemies as well
    // private HashSet<Collider2D> hitTargets = new HashSet<Collider2D>();

    //private float stompDelay = 0.5f;
    //private float stompTimer = 0f;
    //private bool hasStomped = false;

    public CyberBossStompAttack(Entity new_entity) : base(new_entity)
    {
        cyberBoss = (CyberBoss) new_entity;
    }

    public override void EnterState(Dictionary<string, object> args = null)
    {
        base.EnterState(args);

        aoeCollider.enabled = false;
        isShockwaveActive = false;
        currentRadius = 0f;

        EmitStomp(); // stomp happens upon entering
    }

    public override void UpdateState()
    {
        cyberBoss.rigidBody.linearVelocity = Vector2.zero; // cyberboss stays still

        if (isShockwaveActive)
        { 
            // expand the shockwave
            currentRadius += shockwaveSpeed * Time.deltaTime;
            aoeCollider.radius = currentRadius;

            // stop when reached max radius
            if (currentRadius >= maxShockwaveRadius)
            {
                isShockwaveActive = false;
                aoeCollider.enabled = false;
                // hitTargets.Clear();
            }
        }

        /*if (aoeCollider.enabled)
        {
            timer += Time.deltaTime;
            if (timer > TTL)
            {
                timer = 0f;
                aoeCollider.enabled = false;
            }
        }*/
    }

    public void EmitStomp()
    {
        // enable aoe collider
        //if (!aoeCollider.enabled) 
        //    aoeCollider.enabled = true;

        if (!isShockwaveActive)
        {
            isShockwaveActive = true;
            currentRadius = 0.5f; // start with small radius
            aoeCollider.radius = currentRadius;
            aoeCollider.enabled = true;
        }
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        // initialize stomp velocity - looking for player
        if (collision.gameObject.TryGetComponent<Player>(out var player))
        {
            player.pushed = true; // player has been pushed
            Vector2 pushDirection = player.GetDirectionToPosition(cyberBoss.transform.position);
            player.pushedVelocity = pushForce * pushDirection;
        }
    }

    public override void ExitState(Dictionary<string, object> args = null)
    {
        base.ExitState(args);
        isShockwaveActive = false;
        aoeCollider.enabled = false;
    }

}

using UnityEngine;
using System.Collections.Generic;
using Unity.VisualScripting.FullSerializer;

public class CyberBossStompAttack: State
{
    readonly CyberBoss cyberBoss;
    public Stomp stomp;

    //private const float TTL = 0.3f;
    //private float timer = 0f;

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
        //aoeCollider = cyberBoss.capsuleCollider2D;

       // stomp = cyberBoss.stomp;

    }

    public override void EnterState(Dictionary<string, object> args = null)
    {
        cyberBoss.rigidBody.linearVelocity = Vector2.zero;
        cyberBoss.stomp.EmitPush();
    }

    public override void UpdateState()
    {
        if (!cyberBoss.stomp.aoeCollider.enabled)
        {
            cyberBoss.ChangeState("Idle");
       }
    }


    public override void ExitState(Dictionary<string, object> args = null)
    {

    }

}

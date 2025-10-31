using System.Collections.Generic;
using UnityEngine;

public class SniperHide : State
{
    private const float timeToStep = 0.25f;

    readonly Sniper sniper;
    private float hideTime;
    private float deltaCount = 0f;
    int stepCounter = 1;

    private RaycastHit2D hit;
    private float directionTimer = 0f;
    readonly string[] layerNames = { "Player", "Default" };
    private LayerMask layersToAvoid;


    public SniperHide(Entity new_entity) : base(new_entity)
    {
        sniper = (Sniper)new_entity;
    }

    public override void EnterState(Dictionary<string, object> args = null)
    {
        sniper.timer = 0f;
        directionTimer = 0f;

        deltaCount = 0f;
        stepCounter = 1;

        sniper.Invisible();
        hideTime = Random.Range(sniper.invisibleTimeRange[0], sniper.invisibleTimeRange[1]);

        layersToAvoid = LayerMask.GetMask(layerNames);
    }

    public override void UpdateState()
    {
        if (sniper.target == null)
        {
            Debug.LogError("Target not found");
            return;
        }

        deltaCount += Time.deltaTime;

        // movement
        sniper.rigidBody.linearVelocity = -sniper.gameObject.transform.right.normalized * sniper.entityData.moveSpeed;
        if (sniper.PlayFootsteps(deltaCount, timeToStep, stepCounter))
        {
            deltaCount = 0f;
            stepCounter++;
            if (stepCounter > 3) stepCounter = 1;
        }

        directionTimer += Time.deltaTime;

        hit = Physics2D.Raycast(sniper.firePoint.transform.position, -sniper.gameObject.transform.right.normalized, 5f, layersToAvoid);
        
        if (hit.collider != null || directionTimer > 0.35f) 
        {
            directionTimer = 0f;

            // change direction of movement
            Quaternion rotation = sniper.gameObject.transform.rotation;

            rotation.z += Random.Range(-1f, 1f);

            sniper.gameObject.transform.rotation = rotation;
        }

        if (sniper.timer > hideTime)
            sniper.ChangeState("Idle");
    }
    
    public override void ExitState(Dictionary<string, object> args = null)
    {
        sniper.timer = 0;
        sniper.Visible();
    }
}
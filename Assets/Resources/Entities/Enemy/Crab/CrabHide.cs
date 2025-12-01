using System.Collections.Generic;
using UnityEngine;

public class CrabHide : State
{
    private const float timeToStep = 0.25f;

    readonly Crab crab;
    private float hideTime;
    private float deltaCount = 0f;
    int stepCounter = 1;

    private RaycastHit2D hit;
    private float directionTimer = 0f;
    readonly string[] layerNames = { "Player", "Default" };
    private LayerMask layersToAvoid;


    public CrabHide(Entity new_entity) : base(new_entity)
    {
        crab = (Crab)new_entity;
    }

    public override void EnterState(Dictionary<string, object> args = null)
    {
        crab.timer = 0f;
        directionTimer = 0f;

        deltaCount = 0f;
        stepCounter = 1;

        crab.Invisible();
        hideTime = Random.Range(crab.invisibleTimeRange[0], crab.invisibleTimeRange[1]);

        layersToAvoid = LayerMask.GetMask(layerNames);

        crab.PlayAnim("Idle");
    }

    public override void UpdateState()
    {
        if (crab.target == null)
        {
            Debug.LogError("Target not found");
            return;
        }

        deltaCount += Time.deltaTime;

        // movement
        crab.moveVelocity = -crab.gameObject.transform.right.normalized * crab.entityData.moveSpeed;
        if (crab.PlayFootsteps(deltaCount, timeToStep, stepCounter))
        {
            deltaCount = 0f;
            stepCounter++;
            if (stepCounter > 3) stepCounter = 1;
        }

        directionTimer += Time.deltaTime;

        hit = Physics2D.Raycast(crab.firePoint.transform.position, -crab.gameObject.transform.right.normalized, 5f, layersToAvoid);
        
        if (hit.collider != null || directionTimer > 0.35f) 
        {
            directionTimer = 0f;

            // change direction of movement
            Quaternion rotation = crab.gameObject.transform.rotation;

            rotation.z += Random.Range(-1f, 1f);

            crab.gameObject.transform.rotation = rotation;
        }

        if (crab.timer > hideTime)
            crab.ChangeState("Idle");
    }
    
    public override void ExitState(Dictionary<string, object> args = null)
    {
        crab.timer = 0;
        crab.Visible();
    }
}
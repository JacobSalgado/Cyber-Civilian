using System.Collections.Generic;
using NUnit.Framework;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Audio;
using UnityEngine.InputSystem;

public class Player : Entity
{
    public InputActionReference moveAction;

    public override void InitializeStates()
    {
        AddState("Idle", new PlayerIdle(this));
        AddState("Move", new PlayerMove(this));
        current_state = stateMap["Idle"];
    }

    void Start()
    {
        InitializeStates();
        //current_state = stateMap["Idle"];
    }

    void Update()
    {
        //velocity = moveAction.action.ReadValue<Vector2>();
    }

    void FixedUpdate()
    {
        current_state.UpdateState();
    }

    /*
    private enum SFXNames
    {
        STEP1,
        STEP2,
        STEP3
    }

    public InputActionReference moveAction;
    private float deltaCount = 0f;
    private int stepCounter = 0;

    public Vector2 velocity;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
    }

    void PlayFootsteps(float delta)
    {
        if (deltaCount > stepSFXDuration || deltaCount == 0f)
        {
            SFXPlayer.PlayOneShot(entityData.SFX[stepCounter]);
            deltaCount = 0f;
            stepCounter++;
            if (stepCounter % 3 == 0)
                stepCounter = 0;
        }
        deltaCount += delta;
    }

    // Update is called once per frame
    void Update()
    {
        velocity = moveAction.action.ReadValue<Vector2>();
    }

    // Called once per physics time step    
    void FixedUpdate()
    {
        rb.linearVelocity = new Vector2(velocity.x * entityData.moveSpeed, velocity.y * entityData.moveSpeed);
        if (rb.linearVelocity != Vector2.zero)
            PlayFootsteps(Time.deltaTime);
        else
        {
            deltaCount = 0f;
        }
    }
    */
}

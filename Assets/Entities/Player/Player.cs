using System.Collections.Generic;
using NUnit.Framework;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Audio;
using UnityEngine.InputSystem;

public class NewMonoBehaviourScript : MonoBehaviour
{
    private enum SFXNames
    {
        STEP1,
        STEP2,
        STEP3
    }

    public InputActionReference moveAction;
    public Entity player;

    private float deltaCount = 0f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        player = GetComponent<Entity>(); // access Entity class data
    }

    void PlayFootsteps(float delta)
    {
        deltaCount += delta;
        if (deltaCount > 1f)
        {
            player.SFXSource.PlayOneShot(player.SFX[0]);
            deltaCount = 0f;
        }
    }

    // Update is called once per frame
    void Update()
    {
        player.movement = moveAction.action.ReadValue<Vector2>();
    }

    //Called once per physics time step    
    void FixedUpdate()
    {
        player.rb.linearVelocity = new Vector2(player.movement.x * player.moveSpeed, player.movement.y * player.moveSpeed);
        if (player.rb.linearVelocity != Vector2.zero)
        {
            PlayFootsteps(Time.deltaTime);
        }
    }
}

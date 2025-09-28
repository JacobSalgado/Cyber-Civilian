//using System.Numerics;
using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : Entity
{
    [NonSerialized] public Camera cam;
    public InputActionReference moveAction;
    Vector2 mousePos;

    public override void InitializeStates()
    {
        AddState("Idle", new PlayerIdle(this));
        AddState("Move", new PlayerMove(this));
        AddState("Death", new EntityDeath(this));

        ChangeState("Idle");
    }

    void Start()
    {
        InitializeStates();
    }

    void Update()
    {
        mousePos = cam.ScreenToWorldPoint(Mouse.current.position.ReadValue());
    }

    public override void FixedUpdate()
    {
        base.FixedUpdate();

        Vector2 lookDir = mousePos - rigidBody.position;
        float angle = Mathf.Atan2(lookDir.y, lookDir.x) * Mathf.Rad2Deg - 90f;
        rigidBody.rotation = angle;
    }
}

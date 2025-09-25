//using System.Numerics;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : Entity
{
    public Camera cam;
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
        //healthBar = new HealthBar(entityData, );
    }

    void Update()
    {
        mousePos = cam.ScreenToWorldPoint(Mouse.current.position.ReadValue());
    }

    void FixedUpdate()
    {
        current_state.UpdateState();

        Vector2 lookDir = mousePos - rb.position;
        float angle = Mathf.Atan2(lookDir.y, lookDir.x) * Mathf.Rad2Deg - 90f;
        rb.rotation = angle;
    }
}

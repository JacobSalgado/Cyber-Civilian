//using System.Numerics;
using System;
using System.Collections;
//using System.Numerics;
using NUnit.Framework;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : Entity
{
    public enum PlayerWeaponType
    {
        BULLET,
        RAILGUN,
        MISSILE,
        PLASMA,
        FLAMETHROWER,
        NONE
    }

    [NonSerialized] public Camera cam;

    // Used to create dashing effect
    [SerializeField] public TrailRenderer tr;

    [Header("Controls")]
    public InputActionReference moveAction;
    public InputActionReference fireAction;
    public InputActionReference dashAction;
    private Vector2 mousePos;

    [Header("Shooting Properties")]
    [SerializeField] private GameObject[] weapons;
    public PlayerWeaponType currentWeaponType;

    // TODO: assign weapons to player in inspector

    [Header("Dashing properties")]
    private bool canDash = true;
    private bool isDashing = false;
    [SerializeField] public float dashPower = 2f;
    [SerializeField] public float dashTime = 0.2f;
    [SerializeField] public float dashCooldown = 1f;



    public override void InitializeStates()
    {
        AddState("Idle", new PlayerIdle(this));
        AddState("Move", new PlayerMove(this));
        AddState("Dash", new PlayerDash(this));

        ChangeState("Idle");
    }

    public override void Start()
    {
        base.Start();
        InitializeStates();
        tr.emitting = false;
    }

    void Update()
    {
        // update mouse position
        mousePos = cam.ScreenToWorldPoint(Mouse.current.position.ReadValue());

        // check fire inputs
        //weapons[(int)currentWeaponType].GetComponent<Weapon>().ShootWeapon(fireAction, firePoint, 6);
        
        // check dash inputs
        if (dashAction.action.WasPressedThisFrame() && canDash && !isDashing)
        {
            canDash = false;
            isDashing = true;
            invincibility = true;
            ChangeState("Dash");
        }
    }

    public override void FixedUpdate()
    {
        base.FixedUpdate();

        // update rotation
        Vector2 lookDir = mousePos - rigidBody.position;
        float angle = Mathf.Atan2(lookDir.y, lookDir.x) * Mathf.Rad2Deg - 180f;
        rigidBody.rotation = angle;
    }

    public override void OnCollisionEnter2D(Collision2D collision)
    {
        
    }

    public bool getIsDashing()
    {
        return isDashing;
    }

    public void setCanDash(bool new_canDash)
    {
        canDash = new_canDash;
    }

    public void setIsDashing(bool new_isDashing)
    {
        isDashing = new_isDashing;
    }
}

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
    public enum PlayerProjType
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
    [SerializeField] private GameObject[] projPrefabs;
    [SerializeField] private float fireRate = 5f;
    [SerializeField] private Transform firePoint;
    private float fireTimer = 0f;
    public PlayerProjType currentProj;

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

    void Start()
    {
        InitializeStates();
        tr.emitting = false;
    }

    void Update()
    {
        // update mouse position
        mousePos = cam.ScreenToWorldPoint(Mouse.current.position.ReadValue());

        // check fire inputs
        if (fireAction.action.IsPressed())
        {
            Debug.Log("FIRE");
            fireTimer -= Time.deltaTime;
            if (fireTimer <= 0f)
            {
                Shoot();
                fireTimer += 1f / fireRate;
            }
        }
        else
        {
            fireTimer = 0f;
        }

        // check dash inputs
        if (dashAction.action.WasPressedThisFrame() && canDash && !isDashing)
        {
            Debug.Log("DASH");
            canDash = false;
            isDashing = true;
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

    void Shoot()
    {
        Projectile proj = Instantiate(projPrefabs[(int)currentProj], firePoint.position, firePoint.rotation).GetComponent<Projectile>();
        proj.mousePos = mousePos;
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

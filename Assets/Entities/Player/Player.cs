//using System.Numerics;
using System;
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

    [Header("Controls")]
    public InputActionReference moveAction;
    public InputActionReference fireAction;
    private Vector2 mousePos;

    [Header("Shooting Properties")]
    [SerializeField] private GameObject[] projPrefabs;
    [SerializeField] private float fireRate = 5f;
    [SerializeField] private Transform firePoint;
    private float fireTimer = 0f;
    public PlayerProjType currentProj;


    public override void InitializeStates()
    {
        AddState("Idle", new PlayerIdle(this));
        AddState("Move", new PlayerMove(this));

        ChangeState("Idle");
    }

    void Start()
    {
        InitializeStates();
    }

    void Update()
    {
        // update mouse position
        mousePos = cam.ScreenToWorldPoint(Mouse.current.position.ReadValue());

        // check fire inputs
        if (fireAction.action.IsPressed())
        {
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
}

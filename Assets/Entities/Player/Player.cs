using System;
using System.Collections;
using NUnit.Framework;
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

    // private variables
    [NonSerialized] public Camera cam;
    private Vector2 mousePos;

    [Header("==Controls==")]
    public InputActionReference moveAction;
    public InputActionReference fireAction;
    public InputActionReference dashAction;
    public InputActionReference weaponKeybindsAction;
    public InputActionReference phaseAction;

    [Header("==Weapon Properties==")]
    [SerializeField] private GameObject[] weapons;
    public PlayerWeaponType currentWeaponType = PlayerWeaponType.BULLET;
    public SpriteRenderer weaponRenderer; // Renderer for switching weapon sprites

    [Header("==Dashing Properties==")]
    public float dashPower = 2f;
    public float dashTime = 0.2f;
    public float dashCooldown = 1f;
    private bool canDash = true;
    private bool isDashing = false;
    private bool isPhasing = false;

    [Header("==Trail Renderer==")]
    public TrailRenderer tr; // Used to create dashing effect

    [Header("==Resource==")]

    public float energy = 1000f;
    public float energyStep = 0.01f;
    public float dashCost = 100f;
    public float phaseCost = 0.1f;

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

        EquipNewWeapon(currentWeaponType);
    }

    void Update()
    {
        // update mouse position
        mousePos = cam.ScreenToWorldPoint(Mouse.current.position.ReadValue());

        // check for weaponKeybind input
        if (weaponKeybindsAction.action.WasPressedThisFrame())
        {
            // check which button was pressed in the actionMap
            PlayerWeaponType new_weapon_type;
            string action = weaponKeybindsAction.action.activeControl.name;

            if (string.Compare(action, "q") != 0)
            {
                int num_key = int.Parse(action);
                new_weapon_type = (PlayerWeaponType)(num_key - 1);
            }
            else new_weapon_type = (PlayerWeaponType)(((int)currentWeaponType + 1) % weapons.Length);

            EquipNewWeapon(new_weapon_type);
        }

        // check fire inputs
        if (currentWeaponType < PlayerWeaponType.NONE)
            ShootWeapon(weapons[(int)currentWeaponType], fireAction, firePoint, 6);

        // check dash inputs
        if (dashAction.action.WasPressedThisFrame() && canDash && !isDashing && energy - dashCost >= 0)
        {
            canDash = false;
            isDashing = true;
            invincibility = true;
            energy -= dashCost;
            ChangeState("Dash");
        }

        // check phase inputs
        if (phaseAction.action.WasPressedThisFrame() && !isPhasing && energy - phaseCost >= 0)
            Phase(true);
        else if (phaseAction.action.WasPressedThisFrame() && isPhasing)
            Phase(false);
    }

    public override void FixedUpdate()
    {
        base.FixedUpdate();

        // update rotation
        Vector2 dir = GetDirectionToPosition(mousePos);
        RotateToDirection(dir);

        // regenerate energy
        if (energy < 1000)
        {
            energy += energyStep;
            if (energy > 1000) energy = 1000;
        }

        if (isPhasing && energy <= 0)
        {
            Phase(false);
        }
        else if (isPhasing)
        {
            energy -= phaseCost;
        }
    }

    public void EquipNewWeapon(PlayerWeaponType newWeaponType)
    {
        if ((int)newWeaponType < weapons.Length)
        {
            currentWeaponType = newWeaponType;
            weaponRenderer.sprite = weapons[(int)newWeaponType].GetComponent<Weapon>().weaponSprite;
        }
        else Debug.LogError(string.Format("{0} not in weapon array", newWeaponType));
    }

    public void Phase(bool activate)
    {
        if (activate)
        {
            isPhasing = true;
            //string[] ignored_layers = { "Default" };
            Debug.Log("Change state to phasing");
            //LayerMask phaseLayer = LayerMask.GetMask("Default");
            //rigidBody.excludeLayers = phaseLayer;
            gameObject.layer = 8; // Layer Phasing
        }
        else
        {
            isPhasing = false;
            Debug.Log("Change state to normal");
            gameObject.layer = 0; // Layer Default
        }
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

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

    public override void InitializeStates()
    {
        AddState("Idle", new PlayerIdle(this));
        AddState("Move", new PlayerMove(this));
        AddState("Dash", new PlayerDash(this));

        ChangeState("Idle");
    }

    public override void Start()
    {
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
        if (dashAction.action.WasPressedThisFrame() && canDash && !isDashing)
        {
            canDash = false;
            isDashing = true;
            invincibility = true;
            ChangeState("Dash");
        }

        // check phase inputs
        if (phaseAction.action.WasPressedThisFrame() && !isPhasing)
            Phase(true);
        else if (phaseAction.action.WasPressedThisFrame() && isPhasing)
            Phase(false);
    }

    public override void FixedUpdate()
    {
        base.FixedUpdate();

        // update rotation
        Vector2 lookDir = mousePos - rigidBody.position;
        float angle = Mathf.Atan2(lookDir.y, lookDir.x) * Mathf.Rad2Deg - 180f;
        rigidBody.rotation = angle;
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
            gameObject.layer = LayerMask.NameToLayer("Phasing"); // Layer 8
        }
        else
        {
            isPhasing = false;
            gameObject.layer = LayerMask.NameToLayer("Default"); // Layer 0
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

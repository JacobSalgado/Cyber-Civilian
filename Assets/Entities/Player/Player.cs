using System;
using System.Collections;
using JetBrains.Rider.Unity.Editor;
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

    [Header("==Necessary GameObjects==")]
    public TrailRenderer tr; // Used to create dashing effect

    [Header("==Controls==")]
    public InputActionReference moveAction;
    public InputActionReference fireAction;
    public InputActionReference dashAction;
    public InputActionReference weaponKeybindsAction;

    [Header("==Weapon Properties==")]
    public Transform firePoint;
    [SerializeField] private GameObject[] weapons;
    public PlayerWeaponType currentWeaponType = PlayerWeaponType.BULLET;
    public SpriteRenderer weaponRenderer; // Renderer for switching weapon sprites

    [Header("==Dashing Properties==")]
    public float dashPower = 2f;
    public float dashTime = 0.2f;
    public float dashCooldown = 1f;
    private bool canDash = true;
    private bool isDashing = false;

    // private variables
    [NonSerialized] public Camera cam;
    [NonSerialized] public PlayerData playerData;
    private Vector2 mousePos;

    public override void InitializeStates()
    {
        AddState("Idle", new PlayerIdle(this));
        AddState("Move", new PlayerMove(this));
        AddState("Dash", new PlayerDash(this));

        ChangeState("Idle");
    }

    public override void EntityDie()
    {
        // TODO: GAME OVER SCREEN
    }

    public override void Start()
    {
        base.Start();
        playerData = (PlayerData)entityData;
        //Debug.Log(playerData.test);

        InitializeStates();
        tr.emitting = false;

        for (int i = 0; i < weapons.Length; i++) {
            weapons[i] = Instantiate(weapons[i]);
            weapons[i].transform.SetParent(transform);
        }

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
    }

    public override void FixedUpdate()
    {
        base.FixedUpdate();

        // update rotation
        Vector2 dir = GetDirectionToPosition(mousePos);
        RotateToDirection(dir);
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

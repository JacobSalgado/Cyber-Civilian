using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

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
    public SpriteRenderer spriteRenderer;
    public Sprite shieldSprite;

    [Header("==Controls==")]
    public InputActionReference moveAction;
    public InputActionReference fireAction;
    public InputActionReference dashAction;
    public InputActionReference shieldAction;
    public InputActionReference weaponKeybindsAction;
    public InputActionReference phaseAction;

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
    private bool isPhasing = false;

    [Header("==Resource Properties==")]
    // TODO: set resource regen timer
    public int currentEnergy = 1000;
    public int maxEnergy = 1000;
    public int energyRegen = 1;
    public int dashCost = 100;
    public int phaseCost = 50;

    [Header("==Blocking Properties==")]
    public float shieldDrainRate = 50f;

    // NonSerialized variables
    [NonSerialized] public Camera cam;
    [NonSerialized] public PlayerData playerData;
    [NonSerialized] public Slider resourceMeter;
    [NonSerialized] public bool updatePlayer = false;
    [NonSerialized] public Dictionary<string, object> updateArgs;
    [NonSerialized] public float damagedTimer = 0f;
    [NonSerialized] public float damagedTime = 0f;
    private Vector2 mousePos;
    private bool canBlock = true;
    private bool isBlocking = false;
    private PlayerWeaponType previousWeaponType;

    public override void InitializeStates()
    {
        AddState("Idle", new PlayerIdle(this));
        AddState("Move", new PlayerMove(this));
        AddState("Dash", new PlayerDash(this));

        ChangeState("Idle");
    }

    public override void EntityDie()
    {
        isDead = true;
        gameObject.SetActive(false);
    }

    public override void Start()
    {
        base.Start();
        playerData = (PlayerData)entityData;
        //Debug.Log(playerData.test);

        InitializeStates();
        tr.emitting = false;

        for (int i = 0; i < weapons.Length; i++)
        {
            weapons[i] = Instantiate(weapons[i], transform);
            weapons[i].GetComponent<Weapon>().owner = this;
        }

        EquipNewWeapon(currentWeaponType);

        if (updatePlayer && playerData != null)
        {
            playerData.currentHealth = (int)updateArgs["currentPlayerHealth"];
            playerData.maxHealth = (int)updateArgs["currentPlayerMaxHealth"];
            UpdateHealthBar();

            updatePlayer = false;
            updateArgs = null;
        }
    }

    void Update()
    {
        if (isDamaged)
        {
            damagedTimer += Time.deltaTime;
            if (damagedTimer > damagedTime)
            {
                damagedTime = 0f;
                isDamaged = false;  
                damagedTimer = 0f;
            }
        }

        // update mouse position
        mousePos = cam.ScreenToWorldPoint(Mouse.current.position.ReadValue());

        // check for weaponKeybind input
        if (weaponKeybindsAction.action.WasPressedThisFrame())
        {
            // Resets from previously having the shield
            canBlock = true;
            isBlocking = false;
            invincibility = false;

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
        if (currentWeaponType < PlayerWeaponType.NONE && !isBlocking)
            ShootWeapon(weapons[(int)currentWeaponType], fireAction, firePoint, 6);

        // check dash inputs
        if (dashAction.action.WasPressedThisFrame() && canDash && !isDashing && currentEnergy - dashCost >= 0)
        {
            canDash = false;
            isDashing = true;
            invincibility = true;
            currentEnergy -= dashCost;
            ChangeState("Dash");
        }

        // check phase inputs
        if (phaseAction.action.WasPressedThisFrame() && !isPhasing && currentEnergy - phaseCost >= 0)
        {
            Phase(true);
        }
        else if (phaseAction.action.WasPressedThisFrame() && isPhasing)
        {
            Phase(false);
        }

        // check shield inputs
        if (shieldAction.action.WasPressedThisFrame())
        {
            if (isBlocking)
            {
                canBlock = true;
                isBlocking = false;
                EquipNewWeapon(previousWeaponType);
            }
            else if (canBlock && !isBlocking)
            {
                previousWeaponType = currentWeaponType;

                canBlock = false;
                isBlocking = true;
                EquipShield();
            }
        }

        // regenerate energy
        if (currentEnergy < 1000 && !isBlocking && !isPhasing)
        {
            currentEnergy += energyRegen;
            if (currentEnergy > 1000) currentEnergy = 1000;
        }

        if (isPhasing)
        {
            if (currentEnergy <= 0)
                Phase(false);
            else
                currentEnergy -= phaseCost;
        }

        if (isBlocking)
        {
            currentEnergy -= Mathf.RoundToInt(shieldDrainRate * Time.deltaTime);
            if (currentEnergy <= 0)
            {
                currentEnergy = 0;
                canBlock = true; // reset for when resource regenerates
                isBlocking = false;
                // switch from shield to gun
                EquipNewWeapon(previousWeaponType);
            }
        }

        // UI updates
        UpdateResourceMeter();
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
        else Debug.LogError($"{newWeaponType} not in weapon array");
    }

    public void UpdateResourceMeter()
    {
        resourceMeter.maxValue = maxEnergy;
        resourceMeter.value = currentEnergy;
    }

    // Phase functions
    public void Phase(bool activate)
    {
        if (activate)
        {
            audioManager.PlayAudioSource("PhaseStart");
            isPhasing = true;
            changeSpriteAlpha(spriteRenderer, 0.35f);
            changeSpriteAlpha(weaponRenderer, 0.35f);
            gameObject.layer = 3;
            Debug.Log("Change state to phasing");
        }
        else
        {
            audioManager.PlayAudioSource("PhaseEnd");
            isPhasing = false;
            changeSpriteAlpha(spriteRenderer, 1f);
            changeSpriteAlpha(weaponRenderer, 1f);
            gameObject.layer = 6;
            Debug.Log("Change state to normal");
        }
    }

    public void changeSpriteAlpha(SpriteRenderer sr, float new_alpha)
    {
        Color tempColor = sr.color;
        tempColor.a = new_alpha;
        sr.color = tempColor;
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

    // Shield Functions
    public void EquipShield()
    {
        // change weaponrenderer sprite to a shield sprite
        weaponRenderer.sprite = shieldSprite;
    }

    public bool getIsBlocking()
    {
        return isBlocking;
    }

    public void setCanBlock(bool new_canBlock)
    {
        isBlocking = new_canBlock;
    }

    public void setIsBlocking(bool new_isBlocking)
    {
        isBlocking = new_isBlocking;
    }

    public override void GotDamaged()
    {
        base.GotDamaged();

        if (isDamaged && damagedTime == 0f)
        {
            string name = $"Damaged{UnityEngine.Random.Range(1, 5)}";
            audioManager.PlayAudioSource(name);
            damagedTime = audioManager.audioEffects[name].clip.length;
        }
    }
}

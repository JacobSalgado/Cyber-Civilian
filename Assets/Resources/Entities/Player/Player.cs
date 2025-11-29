using System;
using System.Collections.Generic;
using TMPro;
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
    public TrailRenderer trailRenderer; // Used to create dashing effect
    public SpriteRenderer spriteRenderer;
    public ChargeMeter chargeMeter;

    [Header("Ability GameObjects")]
    public PlayerPush pushAbility;
    public PlayerPhase phaseAbility;
    public PlayerShield shieldAbility;
    public PlayerVortex vortexAbility;

    [Header("==Input Maps==")]
    [SerializeField] private PlayerInputManager inputManager;
    public InputActionReference moveAction;
    public InputActionReference fireAction;
    public InputActionReference dashAction;
    public InputActionReference shieldAction;
    public InputActionReference weaponKeybindsAction;
    public InputActionReference phaseAction;
    public InputActionReference reloadAction;
    public InputActionReference vortexAction;
    public InputActionReference pushAction;

    [Header("==Weapon Properties==")]
    [NonSerialized] public Weapon currentWeapon;
    public Transform firePoint;
    public GameObject[] weapons;
    public PlayerWeaponType currentWeaponType = PlayerWeaponType.BULLET;
    public SpriteRenderer weaponRenderer; // Renderer for switching weapon sprites

    [Header("==Dashing Properties==")]
    public float dashPower = 2f;
    public float dashTime = 0.2f;
    public float dashCooldown = 1f;

    /* Input Action Flags */
    [NonSerialized] public bool isDashing = false;
    [NonSerialized] public bool isPhasing = false;
    [NonSerialized] public bool isReloading = false;
    [NonSerialized] public bool isShielding = false;
    [NonSerialized] public bool isVortexing = false;

    [Header("==Resource Properties==")]
    public int currentEnergy = 1000;
    public int maxEnergy = 1000;
    public int energyRegen = 10;
    public int dashCost = 100;
    public int pushCost = 200;

    /* NonSerialized variables */
    // Assigned during runtime
    [NonSerialized] public Camera cam;
    [NonSerialized] public PlayerData playerData;
    [NonSerialized] public Slider resourceMeter;
    [NonSerialized] public TextMeshProUGUI ammoCountText;
    [NonSerialized] public TextMeshProUGUI vortexMultiplierText;
    private Vector2 mousePos;

    // Update parameters
    [NonSerialized] public bool updatePlayer = false;
    [NonSerialized] public Dictionary<string, object> updateArgs;

    // "Damaged" variables for "Damaged" SFX
    [NonSerialized] public float damagedTimer = 0f;
    [NonSerialized] public float damagedTime = 0f;

    // Bonus Damage is received from Vortex Ability
    private const float BONUS_DAMAGE_TIME = 3.0f;
    private float bonusDamageTimer = 0f;
    [NonSerialized] public bool bonusDamageSet = false;
    [NonSerialized] public float bonusDamageMultiplier = 1f;
    
    // Container for resetting previous weapon after finishing certain abilities
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
        trailRenderer.emitting = false;

        // Create copy of weapon GameObject and ensure owner is the player
        for (int i = 0; i < weapons.Length; i++)
        {
            weapons[i] = Instantiate(weapons[i], transform);
            weapons[i].GetComponent<Weapon>().owner = this;
        }

        EquipNewWeapon(currentWeaponType);

        // Update this specific player instance to match the player instance from the previous level
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
        currentWeapon = weapons[(int)currentWeaponType].GetComponent<Weapon>();

        // damage timing for DamagedSFX
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

        // applying bonus damage from Vortex ability
        if (!isVortexing && bonusDamageSet)
        {
            // if (!bonusDamageApplied) {
            //     currentWeapon.projData.damage = (int) Math.Ceiling(currentWeapon.projData.damage * bonusDamageMultiplier);
            //     bonusDamageApplied = true;
            // }

            if (bonusDamageTimer > BONUS_DAMAGE_TIME)
            {
                bonusDamageMultiplier = 1f;
                bonusDamageTimer = 0f;
                bonusDamageSet = false;
                chargeMeter.TurnOffMeter();
            }

            bonusDamageTimer += Time.deltaTime;
        }

        // update mouse position
        mousePos = cam.ScreenToWorldPoint(Mouse.current.position.ReadValue());

        // reloading
        //if (ammo <= 0) Debug.Log("No ammo, reload!");
        if (reloadAction.action.WasPressedThisFrame() && 
            inputManager.CanReload
        ) {
            inputManager.currentInputType = PlayerInputManager.InputType.RELOAD_WEAPON;
        }
    
        // Check for long reload for weapons that support it
        // Currently works bad
        /*
        if (reloadAction.action.IsPressed() && ammo < currentWeapon.maxAmmo && !isShieldBlocking && !isDashing && (currentWeaponType != PlayerWeaponType.BULLET) && (currentWeaponType != PlayerWeaponType.PLASMA))
        {
            if (!isReloading)
            {
                isReloading = true;
                reloadTimer = 0f;
                this.entityData.moveSpeed *= currentWeapon.reloadSlowDownFactor;
                Debug.Log("Reload set to true, long reload started");
            }                                       

            reloadTimer += Time.deltaTime;

            if ((reloadTimer >= currentWeapon.reloadTime || reloadAction.action.WasReleasedThisFrame()) && isReloading)
            {
                isReloading = false;
                this.entityData.moveSpeed = originalSpeed;
                currentWeapon.ReloadWeapon(reloadTimer);
                Debug.Log("Reload set to false, long reload ended");      
            }              
        }
        */

        // check for weaponKeybind input
        if (weaponKeybindsAction.action.WasPressedThisFrame() && inputManager.CanEquip) 
        {
            inputManager.currentInputType = PlayerInputManager.InputType.EQUIP_WEAPON;
        }

        // check fire inputs
        if (currentWeaponType < PlayerWeaponType.NONE &&
            inputManager.CanFire &&
            (fireAction.action.IsPressed() || fireAction.action.WasReleasedThisFrame())
        ) {
            inputManager.currentInputType = PlayerInputManager.InputType.FIRE_WEAPON;
        }

        // check dash inputs
        if (dashAction.action.WasPressedThisFrame() && 
            (currentEnergy - dashCost) >= 0 &&
            !isDashing &&
            IsMoving()
        ) {
            currentEnergy -= dashCost;
            invincibility = true;
            ChangeState("Dash");
        }

        // check phase inputs
        if (phaseAction.action.WasPressedThisFrame())
        {
            inputManager.currentInputType = PlayerInputManager.InputType.PHASE;
        }

        // check shield inputs
        if (shieldAction.action.WasPressedThisFrame())
        {
            inputManager.currentInputType = PlayerInputManager.InputType.SHIELD;
        }

        // check vortex inputs
        if (vortexAction.action.WasPressedThisFrame())
        {
            inputManager.currentInputType = PlayerInputManager.InputType.VORTEX;
        }

        // check push inputs
        if (pushAction.action.WasPressedThisFrame() && currentEnergy - pushCost >= 0)
        {
            audioManager.PlayAudioSource("Push");
            currentEnergy -= pushCost;
            pushAbility.EmitPush();
        }

        // regenerate energy
        if (currentEnergy < maxEnergy && !isShielding && !isPhasing && !isVortexing)
        {
            currentEnergy += (int) Math.Ceiling(energyRegen * Time.deltaTime);
            if (currentEnergy > maxEnergy) currentEnergy = maxEnergy;
        }

        /* UI updates */
        UpdateResourceMeter();
        UpdateAmmoCountText();
        UpdateVortexMultiplierText();
    }

    public override void FixedUpdate()
    {
        base.FixedUpdate();

        // update rotation to mouse position
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

    public void UpdateAmmoCountText()
    {
        if (ammoCountText && currentWeapon) ammoCountText.text = $"({currentWeapon.currentAmmo}/{currentWeapon.maxAmmo})";
    }

    public void UpdateVortexMultiplierText()
    {
        vortexMultiplierText.text = "Vortex Multiplier Damage: " + bonusDamageMultiplier;
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
    public bool IsMoving()
    {
        return moveAction.action.ReadValue<Vector2>() != Vector2.zero;
    }

    // ============================
    // SHIELD FUNCTIONS
    // ============================
    public void EquipShield(bool playAudio = true)
    {
        // change weaponrenderer sprite to a shield sprite
        previousWeaponType = currentWeaponType;
        weaponRenderer.enabled = false;
        isShielding = true;

        if (playAudio)
            audioManager.PlayAudioSource("ShieldStart");
    }

    public void UnequipShield(bool playAudio = true)
    {
        EquipNewWeapon(previousWeaponType);
        weaponRenderer.enabled = true;
        isShielding = false;

        if (playAudio)
            audioManager.PlayAudioSource("ShieldEnd");
    }

    // ===========================
    // VORTEX FUNCTIONS
    // ===========================

    public void StartVortex(bool playAudio = true)
    {
        // change weapon sprite to vortex shield sprite
        previousWeaponType = currentWeaponType;
        isVortexing = true;

        // if (playAudio)
        // audioManager.PlayAudioSource("VortexStart");
    }

    public void StopVortex(float damageMultipler, bool playAudio = true)
    {
        EquipNewWeapon(previousWeaponType);
        isVortexing = false;
        
        if (damageMultipler > 1.0f)
        {
            bonusDamageMultiplier = damageMultipler;
            bonusDamageTimer = 0f;
            bonusDamageSet = true;

            chargeMeter.TurnOnMeter(ChargeMeter.MeterType.BONUS_DAMAGE, BONUS_DAMAGE_TIME, BONUS_DAMAGE_TIME);
        }

        // if (playAudio)
        // audioManager.PlayAudioSource("VortexEnd");
    }
}

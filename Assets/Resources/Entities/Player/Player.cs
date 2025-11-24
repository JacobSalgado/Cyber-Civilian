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
    public TrailRenderer tr; // Used to create dashing effect
    public SpriteRenderer spriteRenderer;
    public PlayerPush pushAbility;
    public PlayerPhase phaseAbility;
    public PlayerShield shieldAbility;
    public PlayerVortex vortexAbility;
    [SerializeField] private PlayerInputManager inputManager;

    [Header("==Input Maps==")]
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
    // TODO: set resource regen timer
    public int currentEnergy = 1000;
    public int maxEnergy = 1000;
    public int energyRegen = 1;
    public int dashCost = 100;
    public int shieldDrainRate = 50;
    public int phaseDrainRate = 50;
    public int pushCost = 200;
    public int vortexCost = 40;
    public int vortexDrainRate = 60;

    [Header("==Blocking Properties==")]
    public float shieldShockDuration = 2f;
    public float shieldSlowDownStrength = 0.5f;
    public int shieldFireDamage = 1;
    public float shieldFireDuration = 3f;

    //[Header("==Vortex Controller==")]
    //public PlayerVortex playerVortex;

    [Header("==Visual Effects==")]
    public GameObject vortexVisualEffect;

    // NonSerialized variables
    [NonSerialized] public Camera cam;
    [NonSerialized] public PlayerData playerData;
    [NonSerialized] public Slider resourceMeter;
    [NonSerialized] public TextMeshProUGUI ammoCountText;
    [NonSerialized] public TextMeshProUGUI vortexMultiplierText;

    [NonSerialized] public bool updatePlayer = false;
    [NonSerialized] public Dictionary<string, object> updateArgs;

    [NonSerialized] public float damagedTimer = 0f;
    [NonSerialized] public float damagedTime = 0f;

    bool bonusDamageSet = false;
    bool bonusDamageApplied = false;
    float bonusDamageTimer = 0f;
    float bonusDamageMultiplier = 1f;
    const float BONUS_DAMAGE_TIME = 3.0f;

    //[NonSerialized] public int ammo;
    [NonSerialized] public Vector2 pushedVelocity; // pushed velocity
    [NonSerialized] public Vector2 moveVelocity = Vector2.zero; // how much player will move
    private Vector2 mousePos;
    private PlayerWeaponType previousWeaponType;

    // other push-related properties
    [NonSerialized] public bool pushed = false;
    private float pushTimer = 0.0f;
    const float PUSHED_TIME = 0.5f;

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

        // check push timing from cyberboss
        /*if (pushed)
        {
            pushTimer += Time.deltaTime;

            // decrease push velocity
            rigidBody.linearVelocity = pushedVelocity;
            pushedVelocity *= 0.85f;

            if (pushTimer > PUSHED_TIME)
            {
                pushTimer = 0f;
                pushed = false;
            }
        }
        else
        {
            rigidBody.linearVelocity = moveVelocity;
        }*/
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
            if (!bonusDamageApplied) {
                currentWeapon.projData.damage = (int) Math.Ceiling((float) currentWeapon.projData.damage * bonusDamageMultiplier);
                bonusDamageApplied = true;
            }

            if (bonusDamageTimer > BONUS_DAMAGE_TIME)
            {
                bonusDamageMultiplier = 1f;
                bonusDamageTimer = 0f;
                bonusDamageSet = false;
                bonusDamageApplied = false;
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
        if (weaponKeybindsAction.action.WasPressedThisFrame() &&
            inputManager.CanEquip
        ) {
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
            !isDashing
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

        // push input checks
        if (pushAction.action.WasPressedThisFrame() && currentEnergy - pushCost >= 0)
        {
            audioManager.PlayAudioSource("Push");
            currentEnergy -= pushCost;
            pushAbility.EmitPush();
        }

        // regenerate energy
        if (currentEnergy < 1000 && !isShielding && !isPhasing && !isVortexing)
        {
            currentEnergy += energyRegen;
            if (currentEnergy > 1000) currentEnergy = 1000;
        }

        // UI updates
        UpdateResourceMeter();
        UpdateAmmoCount();
        UpdateVortexMultiplier();
    }

    public override void FixedUpdate()
    {
        base.FixedUpdate();

        // update rotation
        Vector2 dir = GetDirectionToPosition(mousePos);
        RotateToDirection(dir);

        // apply knockback velocity
        if (pushed)
        {
            pushTimer += Time.deltaTime;

            // decrease knockback velocity
            rigidBody.linearVelocity = pushedVelocity;
            pushedVelocity *= 0.85f;

            if (pushTimer > PUSHED_TIME)
            {
                pushTimer = 0f;
                pushed = false;
            }
        }
        else rigidBody.linearVelocity = moveVelocity;
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

    public void UpdateAmmoCount()
    {
        if (ammoCountText && currentWeapon) ammoCountText.text = $"({currentWeapon.currentAmmo}/{currentWeapon.maxAmmo})";
    }

    public void UpdateVortexMultiplier()
    {
        vortexMultiplierText.text = "Vortex Multiplier Damage: " + bonusDamageMultiplier;
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
    //      Vortex Functions
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
            bonusDamageApplied = false;

        }

        // if (playAudio)
        // audioManager.PlayAudioSource("VortexEnd");
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

    // public void OnCollisionEnter2D(Collision2D collision)
    // {
    //     //Debug.Log("Collided with " + collision.gameObject.name);
    //     // if (isShieldBlocking)
    //     // {
    //     //     if (collision.gameObject.TryGetComponent<Enemy>(out var enemy))
    //     //     {
    //     //         Debug.Log("Applied shock from shield to enemy");
    //     //         enemy.ApplyShockEffect(shiledShockDuration, shieldSlowDownStrength);
    //     //         enemy.ApplyOnFireEffect(shiledFireDuration, shieldFireDamage);
    //     //     }
    //     // }
    // }
}

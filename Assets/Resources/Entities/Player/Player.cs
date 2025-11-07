using System;
using System.Collections;
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
    public Sprite shieldSprite;
    public Sprite vortexSprite;

    [Header("==Controls==")]
    public InputActionReference moveAction;
    public InputActionReference fireAction;
    public InputActionReference dashAction;
    public InputActionReference shieldAction;
    public InputActionReference weaponKeybindsAction;
    public InputActionReference phaseAction;
    public InputActionReference reloadAction;
    public InputActionReference vortexAction;

    [Header("==Weapon Properties==")]
    public Transform firePoint;
    [SerializeField] public GameObject[] weapons;
    public PlayerWeaponType currentWeaponType = PlayerWeaponType.BULLET;
    public Weapon currentWeapon;
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
    private const float timeToCharge = 0.25f;

    [Header("==Blocking Properties==")]
    public float shieldDrainRate = 50f;

    //[Header("==Vortex Controller==")]
    //public PlayerVortex playerVortex;

    [Header("==Vortex Properties==")]
    public float vortexRadius = 2f;
    public int maxAbsorbedProjectiles = 10;
    public float damageMultiplierPerProjectile = 0.2f;
    public float vortexDrainRate = 30f; // Energy drained per second

    [Header("==Visual Effects==")]
    public GameObject vortexVisualEffect;
    public float rotationSpeed = 180f;

    private List<Projectile> absorbedProjectiles = new List<Projectile>();
    private int absorbedCount = 0;
    private GameObject activeVortexEffect;
    private CircleCollider2D vortexCollider;

    // NonSerialized variables
    [NonSerialized] public Camera cam;
    [NonSerialized] public PlayerData playerData;
    [NonSerialized] public Slider resourceMeter;
    [NonSerialized] public TextMeshProUGUI ammoCount;
    [NonSerialized] public bool updatePlayer = false;
    [NonSerialized] public Dictionary<string, object> updateArgs;
    [NonSerialized] public float damagedTimer = 0f;
    [NonSerialized] public float damagedTime = 0f;
    [NonSerialized] public int ammo;
    private Vector2 mousePos;
    public bool isReloading = false;
    // private float reloadTimer = 0f;

    private bool canShieldBlock = true;
    private bool isShieldBlocking = false;

    private bool canVortexBlock = true;
    private bool isVortexBlocking = false;

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

        // create vortex collider
        vortexCollider = gameObject.AddComponent<CircleCollider2D>();
        vortexCollider.radius = vortexRadius;
        vortexCollider.isTrigger = true;
        vortexCollider.enabled = false; // Initially disabled
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

        // reloading
        currentWeapon = weapons[(int)currentWeaponType].GetComponent<Weapon>();
        ammo = currentWeapon.currentAmmo;
        if (ammo <= 0) Debug.Log("No ammo, reload!");
        if (reloadAction.action.WasPressedThisFrame() && ammo < currentWeapon.maxAmmo && !isReloading && !isBlocking && !isDashing)
            Reload();
    
        // Check for long reload for weapons that support it
        // Currently works bad
        /*
        if (reloadAction.action.IsPressed() && ammo < currentWeapon.maxAmmo && !isBlocking && !isDashing && (currentWeaponType != PlayerWeaponType.BULLET) && (currentWeaponType != PlayerWeaponType.PLASMA))
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

        // update mouse position
        mousePos = cam.ScreenToWorldPoint(Mouse.current.position.ReadValue());

        // check for weaponKeybind input
        if (weaponKeybindsAction.action.WasPressedThisFrame())
        {
            // Resets from previously having the shield
            canShieldBlock = true;
            isShieldBlocking = false;
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
        if (currentWeaponType < PlayerWeaponType.NONE && !isShieldBlocking & !isReloading)
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

        // NOTE: for all player abilities except Dash, turn off/stop other abilities first
        // ability use should be mutually-exclusive (if you perform one, you can't do the others)

        // check phase inputs
        if (phaseAction.action.WasPressedThisFrame() && !isPhasing && currentEnergy - phaseCost >= 0)
        {
            if (isBlocking) ShieldEnd();

            Phase(true);
        }
        else if (phaseAction.action.WasPressedThisFrame() && isPhasing)
        {
            Phase(false);
        }

        // check shield inputs
        if (shieldAction.action.WasPressedThisFrame())
        {
            if (isShieldBlocking)
            {
                audioManager.PlayAudioSource("ShieldStart");
                canShieldBlock = true;
                isShieldBlocking = false;
                EquipNewWeapon(previousWeaponType);
            }
            else if (canShieldBlock && !isShieldBlocking)
            {
                previousWeaponType = currentWeaponType;

                audioManager.PlayAudioSource("ShieldEnd");
                canShieldBlock = false;
                isShieldBlocking = true;
                EquipShield();
            }
        }

        // check vortex inputs
        if (vortexAction.action.WasPressedThisFrame())
        {
            if (isVortexBlocking)
            {
                canVortexBlock = true;
                isVortexBlocking = false;

                DeactivateVortex();

                EquipNewWeapon(previousWeaponType);
            }
            else if (canVortexBlock && !isVortexBlocking)
            {
                previousWeaponType = currentWeaponType;

                canVortexBlock = false;
                isVortexBlocking = true;

                ActivateVortex();
                EquipVortex();
            }
        }

        // regenerate energy
        if (currentEnergy < 1000 && !isShieldBlocking && !isPhasing)
        {
            currentEnergy += energyRegen;
            if (currentEnergy > 1000) currentEnergy = 1000;
        }

        /* energy costs for abilities */
        if (isPhasing)
        {
            if (currentEnergy <= 0)
                Phase(false);
            else
                currentEnergy -= phaseCost;
        }

        if (isShieldBlocking)
        {
            currentEnergy -= Mathf.RoundToInt(shieldDrainRate * Time.deltaTime);
            if (currentEnergy <= 0)
            {
                currentEnergy = 0;
                canShieldBlock = true; // reset for when resource regenerates
                isShieldBlocking = false;
                // switch from shield to gun
                EquipNewWeapon(previousWeaponType);
            }
        }

        if (isVortexBlocking)
        {
            //float drainAmount = playerVortex != null ? playerVortex.GetVortexDrainRate() : 30f;
            float drainAmount = GetVortexDrainRate();

            currentEnergy -= Mathf.RoundToInt(drainAmount * Time.deltaTime);

            if (currentEnergy <= 0)
            { 
                currentEnergy = 0;
                canVortexBlock = true; // reset for when resource regenerates
                isVortexBlocking = false;

                // deactivate vortex
                DeactivateVortex();

                EquipNewWeapon(previousWeaponType);
            }
        }

        // UI updates
        UpdateResourceMeter();
        UpdateAmmoCount();

        // rotate visual effect for vortex
        if (activeVortexEffect != null)
        {
            activeVortexEffect.transform.Rotate(0, 0, rotationSpeed * Time.deltaTime);
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
        else Debug.LogError($"{newWeaponType} not in weapon array");
    }

    public void UpdateResourceMeter()
    {
        resourceMeter.maxValue = maxEnergy;
        resourceMeter.value = currentEnergy;
    }

    public void UpdateAmmoCount()
    {
        if (ammoCount && currentWeapon) ammoCount.text = $"({currentWeapon.currentAmmo}/{currentWeapon.maxAmmo})";
    }

    private void ShieldStart()
    {
        previousWeaponType = currentWeaponType;

        audioManager.PlayAudioSource("ShieldStart");
        canBlock = false;
        isBlocking = true;
        EquipShield();
    }

    private void ShieldEnd()
    {
        audioManager.PlayAudioSource("ShieldEnd");
        canBlock = true;
        isBlocking = false;
        EquipNewWeapon(previousWeaponType);
    }

    public void Shield()
    {
        if (isBlocking)
        {
            ShieldEnd();
        }
        else if (canBlock && !isBlocking)
        {
            ShieldStart();
        }
    }

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

    private void Reload()
    {
        isReloading = true;
        // TO-DO: play reload animation here
        StartCoroutine(reloadCoroutine());
    }

    private IEnumerator reloadCoroutine()
    {
        yield return new WaitForSeconds(currentWeapon.reloadTime);
        currentWeapon.ReloadWeapon();
        isReloading = false;
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

    public bool getIsShieldBlocking()
    {
        return isShieldBlocking;
    }

    public void setCanShieldBlock(bool new_canBlock)
    {
        isShieldBlocking = new_canBlock;
    }

    public void setIsShieldBlocking(bool new_isBlocking)
    {
        isShieldBlocking = new_isBlocking;
    }

    // Vortex Functions
    public void EquipVortex()
    {
        // change weaponrenderer sprite to a vortex sprite
        weaponRenderer.sprite = vortexSprite;
    }

    public bool getIsVortexBlocking()
    {
        return isVortexBlocking;
    }

    public void ActivateVortex()
    {
        vortexCollider.enabled = true;
        absorbedCount = 0;
        absorbedProjectiles.Clear();

        // create visual effect
        if (vortexVisualEffect != null)
        {
            activeVortexEffect = Instantiate(vortexVisualEffect, transform);
        }

        Debug.Log("Vortex activated");
    }

    public void DeactivateVortex()
    {
        vortexCollider.enabled = false;

        // destroy visual effect
        if (activeVortexEffect != null)
        {
            Destroy(activeVortexEffect);
        }

        Debug.Log("Vortex deactivated");
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

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // when vortex is active
        if (!vortexCollider.enabled) return;

        // check if enemy projectile
        if (collision.gameObject.TryGetComponent<Projectile>(out var projectile))
        {
            // targeting player layer
            if (projectile.attacking_layer == 6 && absorbedCount < maxAbsorbedProjectiles)
            {
                AbsorbProjectile(projectile.gameObject);
            }
        }
    }

    private void AbsorbProjectile(GameObject projectile)
    {
        absorbedCount++;

        // add visual/audio feedback

        Destroy(projectile);


        Debug.Log($"Absorbed projectile! Count: {absorbedCount}");
    }

    public float GetDamageMultiplier()
    {
        float multiplier = 1f + (absorbedCount * damageMultiplierPerProjectile);
        return multiplier;
    }

    public int GetAbsorbedCount()
    {
        return absorbedCount;
    }

    public void ResetAbsorbedCount()
    {
        absorbedCount = 0;

        // add UI reset and then visual effects here
    }

    public float GetVortexDrainRate()
    {
        return vortexDrainRate;
    }
}

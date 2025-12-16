using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHUD : MonoBehaviour
{
    [Header("==Player HUD Properties==")]
    public Slider healthSlider;
    public Slider resourceSlider;

    // Ammo SubMenu
    [SerializeField] private TextMeshProUGUI currentWeaponText;
    [SerializeField] private TextMeshProUGUI ammoCountText;
    [SerializeField] private Image ammoBackground;

    // Vortex SubMenu
    [SerializeField] private TextMeshProUGUI vortexMultiplierText;
    [SerializeField] private GameObject vortexSubMenu;

    // Level Objective Submenu
    // [SerializeField] private TextMeshProUGUI objectiveText;
    [SerializeField] private Slider enemyKilledProgressBar;

    private readonly Dictionary<Player.WeaponType, Color> weaponColors = new()
    {
        {Player.WeaponType.BULLET, Color.violetRed},
        {Player.WeaponType.RAILGUN, Color.yellow},
        {Player.WeaponType.MISSILE, Color.green},
        {Player.WeaponType.PLASMA, Color.skyBlue},
        {Player.WeaponType.FLAMETHROWER, Color.red},
        {Player.WeaponType.NONE, Color.gray},
    };


    public void PlayerHUDClose()
    {
        Destroy(gameObject);
    }

    public void ToggleVortexSubMenu(bool toggle)
    {
        vortexSubMenu.SetActive(toggle);
    }

    public void UpdateCurrentWeaponType(Player.WeaponType type)
    {
        ammoBackground.color = weaponColors[type];

        currentWeaponText.text = type switch
        {
            Player.WeaponType.BULLET => "PEASHOOTER",
            Player.WeaponType.RAILGUN => "RAILGUN",
            Player.WeaponType.MISSILE => "MISSILE",
            Player.WeaponType.PLASMA => "REVOLVER",
            Player.WeaponType.FLAMETHROWER => "FLAMETHROWER",
            _ => ""
        };

        VertexGradient gradient = currentWeaponText.colorGradient;
        gradient.topLeft = weaponColors[type];
        gradient.topRight = weaponColors[type];
        currentWeaponText.colorGradient = gradient;
    }

    public void UpdateAmmoCountText(int currentAmmo, int maxAmmo)
    {
        ammoCountText.text = $"({currentAmmo}/{maxAmmo})";
    }

    public void UpdateVortexMultiplierText(float multiplier)
    {
        vortexMultiplierText.text = $"{multiplier}%";
    }

    public void UpdateEnemyKilledProgressBar(int current, int max)
    {
        enemyKilledProgressBar.maxValue = max;
        enemyKilledProgressBar.value = current < max ? current : max;
    }
}

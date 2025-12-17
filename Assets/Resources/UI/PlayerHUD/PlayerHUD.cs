using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHUD : MonoBehaviour
{
    [Header("==Player HUD Properties==")]
    public Slider healthSlider;
    public Slider resourceSlider;

    [Header("==Ammo/Weapon SubMenu Properties==")]    
    [SerializeField] private TextMeshProUGUI currentWeaponText;
    [SerializeField] private TextMeshProUGUI ammoCountText;
    [SerializeField] private Image ammoBackground;

    [Header("==Vortex SubMenu Properties==")]
    [SerializeField] private TextMeshProUGUI vortexMultiplierText;
    [SerializeField] private GameObject vortexSubMenu;

    [Header("==Level Progress Properties==")]
    [SerializeField] private TextMeshProUGUI levelObjectiveText;
    [SerializeField] private TextMeshProUGUI enemyKilledText;
    [SerializeField] private Slider enemyKilledProgressBar;

    [Header("==Tutorial Textbox==")]
    [SerializeField] private GameObject tutorialTextbox;
    [SerializeField] private TextMeshProUGUI tutorialText;
    [SerializeField] private TextMeshProUGUI missionText;

    [Header("==HUD VFX==")]
    [SerializeField] private ParticleSystem playerOnFireEffect;
    [SerializeField] private ParticleSystem playerSlowedDownEffect;

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
        enemyKilledText.text = $"({current}/{max})";
    }

    public void SetLevelObjectiveText(string text)
    {
        levelObjectiveText.text = text;
    }

    public void ToggleLevelProgressGameObjects(bool toggle)
    {
        enemyKilledText.gameObject.SetActive(toggle);
        enemyKilledProgressBar.gameObject.SetActive(toggle);
    }

    public void SetMissionText(string text)
    {
        missionText.text = text;
    }

    public void SetTutorialText(string text)
    {
        tutorialText.text = text;
    }

    public void ToggleTutorialTextbox(bool toggle)
    {
        tutorialTextbox.SetActive(toggle);
    }

    public void PlayOnFireEffect()
    {
        playerOnFireEffect.Play();
    }

    public void PlaySlowedEffect()
    { 
        playerSlowedDownEffect.Play();
    }

    public void StopOnFireEffect()
    {
        playerOnFireEffect.Stop();
    }

    public void StopSlowedEffect()
    {
        playerSlowedDownEffect.Stop();
    }
}

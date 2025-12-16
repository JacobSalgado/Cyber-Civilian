using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHUD : MonoBehaviour
{
    [Header("==Player HUD Properties==")]
    public Slider healthSlider;
    public Slider resourceSlider;
    public TextMeshProUGUI ammoCountText;
    public TextMeshProUGUI vortexMultiplierText;
    [SerializeField] private GameObject vortexSubMenu;
    

    /*
    TODO:
    - LEVEL PROGRESSION/MISSION OBJECTION
    - CURRENT WEAPON ICON
    */

    public void PlayerHUDClose()
    {
        Destroy(gameObject);
    }

    public void ToggleVortexSubMenu(bool toggle)
    {
        vortexSubMenu.SetActive(toggle);
    }

    public void UpdateAmmoCountText(int currentAmmo, int maxAmmo)
    {
        ammoCountText.text = $"({currentAmmo}/{maxAmmo})";
    }

    public void UpdateVortexMultiplierText(float multiplier)
    {
        vortexMultiplierText.text = $"{multiplier}%";
    }
}

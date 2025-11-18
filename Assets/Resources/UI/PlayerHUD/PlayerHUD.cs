using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHUD : MonoBehaviour
{
    [Header("Player HUD Properties")]
    public Slider healthSlider;
    public Slider resourceSlider;
    public TextMeshProUGUI ammoCountText;
    public TextMeshProUGUI vortexMultiplierText;

    /*
    TODO:
    - LEVEL PROGRESSION/MISSION OBJECTION
    - CURRENT WEAPON ICON
    */

    public void PlayerHUDClose()
    {
        Destroy(gameObject);
    }
}

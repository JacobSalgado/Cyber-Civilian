using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHUD : MonoBehaviour
{
    [Header("Player HUD Properties")]
    public Slider healthSlider;
    public Slider resourceSlider;
    public TextMeshProUGUI ammoCountText;

    /*
    TODO:
    - AMMO COUNT
    - LEVEL PROGRESSION/MISSION OBJECTION
    */

    public void PlayerHUDClose()
    {
        Destroy(gameObject);
    }
}

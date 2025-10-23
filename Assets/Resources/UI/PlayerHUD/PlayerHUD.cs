using UnityEngine;
using UnityEngine.UI;

public class PlayerHUD : MonoBehaviour
{
    [Header("Player HUD Propertoes")]
    public Slider healthSlider;
    public Slider resourceSlider;

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

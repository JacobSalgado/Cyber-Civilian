using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ChargeMeter : MonoBehaviour
{
    public enum MeterType
    {
        RAILGUN_CHARGE,
        VORTEX_ABSORB,
        BONUS_DAMAGE
    }

    readonly Dictionary<MeterType, string> meterTexts = new()
    {
        {MeterType.RAILGUN_CHARGE, ""},
        {MeterType.VORTEX_ABSORB, ""},
        {MeterType.BONUS_DAMAGE, "Bonus Damage!"},
    };

    public Slider slider;
    public Image fillImage;
    public TextMeshProUGUI sliderText;
    [SerializeField] private Player _player;

    private MeterType type;
    private Vector3 offset = Vector2.zero;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        gameObject.SetActive(false);
        offset = gameObject.transform.localPosition;
    }

    void Update()
    {
        gameObject.transform.SetPositionAndRotation(_player.gameObject.transform.position + offset, Quaternion.identity);
    }

    public void UpdateMeter(float current)
    {
        slider.value = current;
    }

    public void TurnOnMeter(MeterType type, float current, float max)
    {
        this.type = type;
        slider.value = current;
        slider.maxValue = max;
        sliderText.text = meterTexts[this.type];

        gameObject.transform.SetPositionAndRotation(_player.gameObject.transform.position + offset, Quaternion.identity);
        gameObject.SetActive(true);
    }

    public void TurnOffMeter()
    {
        gameObject.SetActive(false);
        slider.value = 0f;
    }
}

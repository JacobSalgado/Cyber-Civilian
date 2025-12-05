using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ChargeMeter : MonoBehaviour
{
    public enum MeterType
    {
        RAILGUN_CHARGE,
        BONUS_DAMAGE,
        RELOADING_WEAPON,
    }

    readonly Dictionary<MeterType, string> meterTexts = new()
    {
        {MeterType.RAILGUN_CHARGE, ""},
        {MeterType.BONUS_DAMAGE, "Damage Up!"},
        {MeterType.RELOADING_WEAPON, "Reloading"},
    };

    public Slider slider;
    public Image fillImage;
    public TextMeshProUGUI sliderText;
    [SerializeField] private Canvas canvas;
    [SerializeField] private Entity _entity;

    private MeterType type;
    private Vector3 offset = Vector2.zero;

    void Start()
    {
        canvas.enabled = false;
        offset = gameObject.transform.localPosition;
    }

    void Update()
    {
        gameObject.transform.SetPositionAndRotation(_entity.gameObject.transform.position + offset, Quaternion.identity);
        
        // TODO: account for more meter types
        if (canvas.enabled){
            if (type == MeterType.BONUS_DAMAGE)
                slider.value -= Time.deltaTime;
            else 
                slider.value += Time.deltaTime;
        }
    }

    // public void UpdateMeter(float current)
    // {
    //     slider.value = current;
    // }

    public void TurnOnMeter(MeterType type, float current, float max)
    {
        this.type = type;
        slider.maxValue = max;
        slider.value = current;
        sliderText.text = meterTexts[this.type];

        gameObject.transform.SetPositionAndRotation(_entity.gameObject.transform.position + offset, Quaternion.identity);
        canvas.enabled = true;
    }

    public void TurnOffMeter()
    {
        canvas.enabled = false;
        slider.value = 0f;
    }
}

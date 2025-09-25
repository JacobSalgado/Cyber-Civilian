using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    public EntityData entityData;
    public Slider slider;

    public HealthBar(EntityData entityData, Slider slider)
    {
        this.entityData = entityData;
        this.slider = slider;

        UpdateHealthBar();
    }

    public void UpdateHealthBar()
    {
        slider.maxValue = entityData.maxHealth;
        slider.value = entityData.currentHealth;
    }
}

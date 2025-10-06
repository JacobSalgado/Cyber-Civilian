using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    public EntityData entityData;
    public Slider slider;

    /// <summary>
    /// backend script that updates a given slider according some entityData
    /// </summary>
    /// <param name="entityData">the data object the slider reads from</param>
    /// <param name="slider">the slider to update</param>
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

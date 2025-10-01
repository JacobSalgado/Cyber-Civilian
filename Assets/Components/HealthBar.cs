using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    public EntityData entityData;
    public Slider slider;

<<<<<<< HEAD
    public GameObject enemy;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
=======
    public HealthBar(EntityData entityData, Slider slider)
    {
        this.entityData = entityData;
        this.slider = slider;

        UpdateHealthBar();
    }

    public void UpdateHealthBar()
>>>>>>> dev
    {
        slider.maxValue = entityData.maxHealth;
        slider.value = entityData.currentHealth;
    }
<<<<<<< HEAD

    // Update is called once per frame
    void FixedUpdate()
    {
        slider.maxValue = entityData.maxHealth;
        slider.value = entityData.currentHealth;
    }

    public void TakeDamage(int amount)
    {
        slider.value -= amount;
    }
=======
>>>>>>> dev
}

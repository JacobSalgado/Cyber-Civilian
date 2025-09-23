using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    [SerializeField]
    public EntityData entityData;

    public Slider slider;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        slider.maxValue = entityData.maxHealth;
        slider.value = entityData.currentHealth;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        slider.maxValue = entityData.maxHealth;
        slider.value = entityData.currentHealth;
    }

    public void TakeDamage(int amount)
    {

    }
}

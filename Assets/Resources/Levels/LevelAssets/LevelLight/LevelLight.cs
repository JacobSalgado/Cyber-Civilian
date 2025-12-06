using UnityEngine;
using UnityEngine.Rendering.Universal;

public class LevelLight : MonoBehaviour
{
    public enum LightBehaviour
    {
        STATIC,
        FLASHING,
        FADE
    }

    [Header("==Light Properties==")]
    [SerializeField] private Light2D light2D;
    [SerializeField] private LightBehaviour behaviour;

    [Header("==Non-Static Properties==")]
    [SerializeField] private float turnOnTime = 0f;
    [SerializeField] private float turnOffTime = 0f;

    private bool turnOn = false;
    private float timer;

    // Update is called once per frame
    void Update()
    {
        // NOTE: static has no dynamic behaviour
        if (behaviour == LightBehaviour.STATIC) return;

        timer += Time.deltaTime;
        NonStaticLight();
    }

    void NonStaticLight()
    {
        if (turnOn)
        {
            if (behaviour == LightBehaviour.FADE)
                light2D.intensity += Time.deltaTime / turnOnTime;

            if (timer > turnOnTime)
            {
                light2D.intensity = 1f;
                turnOn = false;
                timer = 0f;
            }
        }
        else // turnOff
        {
            if (behaviour == LightBehaviour.FADE)
                light2D.intensity -= Time.deltaTime / turnOffTime;

            if (timer > turnOffTime)
            {
                light2D.intensity = 0f;
                turnOn = true;
                timer = 0f;
            }
        }
    }
}

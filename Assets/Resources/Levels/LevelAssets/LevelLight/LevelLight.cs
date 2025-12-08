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
    [SerializeField] private AudioEffect[] SFX;
    [SerializeField] private string SFXToPlay = "";
    public AudioManager audioManager;

    [Header("==Non-Static Properties==")]
    [SerializeField] private float turnOnTime = 0f;
    [SerializeField] private float turnOffTime = 0f;

    private bool turnOn = false;
    private float timer = 0f;

    void Start()
    {
        if (SFX.Length > 0)
            audioManager.InitializeAudioDictionary(SFX);
    }

    void Update()
    {
        // NOTE: static has no dynamic behaviour
        if (behaviour == LightBehaviour.STATIC) return;

        if (audioManager) {
            AudioSource sfx = audioManager.GetAudioSource(SFXToPlay);
            if (sfx && (!sfx.isPlaying || sfx.time > sfx.clip.length - 0.2f))
                audioManager.PlayAudioSource(SFXToPlay);
        }

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

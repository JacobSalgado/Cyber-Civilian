using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [Header("==Necessary GameObjects==")]
    public GameObject audioPlayer;
    public Dictionary<string, AudioSource> audioEffects = new();

    public void InitializeAudioDictionary(AudioEffect[] newAudioEffects)
    {
        for (int i = 0; i < newAudioEffects.Length; i++)
        {
            AudioSource player = audioPlayer.AddComponent<AudioSource>();

            player.playOnAwake = false;
            player.loop = newAudioEffects[i].loop;
            player.pitch = newAudioEffects[i].pitch;
            player.clip = newAudioEffects[i].clip;
            player.volume = newAudioEffects[i].volume;
            player.spatialBlend = newAudioEffects[i].spatialBlend;

            if (player.spatialBlend > 0){
                player.minDistance = newAudioEffects[i].minDistance;
                player.maxDistance = newAudioEffects[i].maxDistance;
                player.spread = 360f;
                player.dopplerLevel = 0f;
            }

            audioEffects.Add(newAudioEffects[i].gameObject.name, player);
        }
    }

    public void PlayAudioSource(string name, float startTime = 0.0f)
    {
        AudioSource player = GetAudioSource(name);
        if (player == null) return;

        if (startTime == 0.0f || (startTime > 0 && !player.isPlaying))
        {
            player.time = startTime;
            player.Play();
        }
    }

    public void PauseAudioSource(string name)
    {
        AudioSource player = GetAudioSource(name);
        if (player != null)
            player.Pause();
    }

    public void UnPauseAudioSource(string name)
    {
        AudioSource player = GetAudioSource(name);
        if (player != null)
            player.UnPause();
    }

    public void StopAudioSource(string name)
    {
        AudioSource player = GetAudioSource(name);
        if (player != null && player.isPlaying)
            player.Stop();
    }
    
    public AudioSource GetAudioSource(string name)
    {
        if (!audioEffects.ContainsKey(name))
        {
            Debug.Log($"{name} not found in audio dictionary");
            return null;
        }

        return audioEffects[name];
    }
}

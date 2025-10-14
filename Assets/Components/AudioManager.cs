using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public AudioSource AudioPlayer;
    public Dictionary<string, AudioClip> audioClips = new();

    public void InitializeAudioDictionary(string[] SFXNames, AudioClip[] SFX)
    {
        for (int i = 0; i < SFX.Length; i++)
        {
            audioClips.Add(SFXNames[i], SFX[i]);
        }
    }

    public void PlayAudioClip(string name, bool loop = false)
    {
        if (!audioClips.ContainsKey(name))
        {
            Debug.Log($"{name} not found in audio dictionary");
            return;
        }

        AudioPlayer.loop = loop;
        AudioPlayer.PlayOneShot(audioClips[name]);
    }
}

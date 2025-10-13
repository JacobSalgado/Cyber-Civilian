using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public AudioSource SFXPlayer;
    public Dictionary<string, AudioClip> SFX = new();

    public void InitializeSFXDictionary(EntityData entityData)
    {
        for (int i = 0; i < entityData.SFX.Length; i++)
        {
            SFX.Add(entityData.SFXNames[i], entityData.SFX[i]);
        }
    }

    public void InitializeSFXDictionary(GameObject gameSFX)
    {
        // TODO: implement game/level sfx
        /*
        for (int i = 0; i < entityData.SFX.Length; i++)
        {
            SFX.Add(entityData.SFXNames[i], entityData.SFX[i]);
        }
        */
    }
}

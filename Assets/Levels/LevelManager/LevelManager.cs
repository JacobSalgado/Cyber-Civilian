using System;
using UnityEditor;
using UnityEngine;

/// <summary>
/// Backend script for managing levels.
/// </summary>
public class LevelManager : MonoBehaviour
{
    [NonSerialized] public Level current_level;

    /// <summary>
    /// Load a level prefab
    /// </summary>
    /// <param name="level_name">Filename of desired level prefab</param>
    public void LoadLevel(string level_name)
    {
        string level_path = string.Format("Assets/Levels/LevelList/{0}.prefab", level_name);

        current_level = PrefabUtility.LoadPrefabContents(level_path).GetComponent<Level>();
        if (current_level)
            current_level.transform.SetParent(this.transform, false);
        else Debug.LogError(string.Format("Level: {0} doesn't exist in LevelList Folder", level_name));
    }

    /*
    TODO: Expand Level Manager Capabilites
    - Restart Level
    - Initiate Enemy Spawns
    - Level Completion Checks
    - etc.
    */
}

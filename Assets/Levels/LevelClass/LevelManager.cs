using System;
using UnityEditor;
using UnityEngine;

/// <summary>
/// </para>Backend script for managing levels.</para>
/// 
/// </para>NOTE: Is a static class, meaning no instantiation and be used globally</para>
/// </summary>
public static class LevelManager
{
    [NonSerialized] public static Level current_level;

    /// <summary>
    /// Load a level prefab
    /// </summary>
    /// <param name="level_name">Filename of desired level prefab</param>
    public static void LoadLevel(string level_name, GameObject parent)
    {
        string level_path = string.Format("Assets/Levels/LevelList/{0}.prefab", level_name);

        current_level = PrefabUtility.LoadPrefabContents(level_path).GetComponent<Level>();
        if (current_level)
            current_level.gameObject.transform.SetParent(parent.transform, false);
        else Debug.LogError(string.Format("Level: {0} doesn't exist in LevelList Folder", level_name));
    }

    public static void Close()
    {
        current_level.LevelClose();
    }

    /*
    TODO: Expand Level Manager Capabilites
    - Restart Level
    - Initiate Enemy Spawns
    - Level Completion Checks
    - etc.
    */
}

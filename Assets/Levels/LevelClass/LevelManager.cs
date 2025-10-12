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
    public static Level current_level;
    public static Player player;
    public static bool isLevelCompleted = false;

    public static int enemyKilledCounter = 0;
    public static int enemyKilledGoal = 2;

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

        player = current_level.player;

        // TODO: START THE LEVEL
        StartLevel();
    }

    public static void StartLevel()
    {
        // assertion check
        if (!current_level)
        {
            Debug.LogError("Current level is null");
            return;
        }

        // Immediate Enemy Spawns Only
        foreach (Transform child in current_level.EnemySpawns.transform)
        {
            EnemySpawn enemySpawn = child.gameObject.GetComponent<EnemySpawn>();
            if (enemySpawn.spawnCondition == EnemySpawn.EnemySpawnCondition.IMMEDIATE)
                enemySpawn.Spawn();
        }
        
        // establish level completion
    }

    public static void Close()
    {
        enemyKilledCounter = 0;
        isLevelCompleted = false;
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

using System.Threading.Tasks;
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

    // Current Level Data
    public static bool isLevelCompleted = false;
    public static int enemyKilledCounter = 0;
    public static float levelTimer = 0;

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

        // Reset LevelManager values
        levelTimer = 0;
        enemyKilledCounter = 0;
        isLevelCompleted = false;

        // When starting a level, only immediate type enemy spawns should be called
        foreach (Transform child in current_level.EnemySpawns.transform)
        {
            EnemySpawn enemySpawn = child.gameObject.GetComponent<EnemySpawn>();
            if (enemySpawn.spawnCondition == EnemySpawn.EnemySpawnCondition.IMMEDIATE)
                enemySpawn.Spawn();
        }
    }

    public static void Update()
    {
        if (current_level == null) return;

        //levelTimer += Time.deltaTime;

        // TODO: spawn non-immediate enemies based on their spawn condition

        // Check if level is completed
        if (current_level.levelObjective == Level.LevelObjective.KILL_ALL_ENEMIES)
        {
            isLevelCompleted = enemyKilledCounter >= current_level.EnemySpawns.transform.childCount;
        }
        else if (current_level.levelObjective == Level.LevelObjective.ENEMY_COUNT)
        {
            isLevelCompleted = enemyKilledCounter >= current_level.enemyKilledGoal;
        }
        else if (current_level.levelObjective == Level.LevelObjective.REACH_EXIT)
        {
            isLevelCompleted = current_level.isExitReached;
        }
    }

    public static void Close()
    {
        levelTimer = 0;
        enemyKilledCounter = 0;
        isLevelCompleted = false;
        Object.Destroy(current_level.gameObject);
    }

    /*
    TODO: Expand Level Manager Capabilites
    - Restart Level
    */
}

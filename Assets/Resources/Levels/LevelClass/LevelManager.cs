using System.Collections;
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
    public static IEnumerator LoadLevel(string level_name, GameManager gameManager)
    {
        string level_path = $"Levels/LevelList/{level_name}/{level_name}";

        ResourceRequest request = Resources.LoadAsync<GameObject>(level_path);
        while (!request.isDone)
        {
            Debug.Log($"loading {level_name}");
            yield return null;
        }

        GameObject instance = gameManager.InstantiatePrefab(request.asset as GameObject, gameManager.levelHolder);

        current_level = instance.GetComponent<Level>();
        if (!current_level)
            Debug.LogError(string.Format("Level: {0} doesn't exist in LevelList Folder", level_name));

        player = current_level.player;

        player.gameObject.SetActive(false);
        current_level.gameObject.SetActive(false);
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

        current_level.gameObject.SetActive(true);
        player.gameObject.SetActive(true);

        // When starting a level, only immediate type enemy spawns should be called
        foreach (Transform child in current_level.EnemySpawns.transform)
        {
            EnemySpawn enemySpawn = child.gameObject.GetComponent<EnemySpawn>();
            if (enemySpawn.spawnCondition == EnemySpawn.EnemySpawnCondition.IMMEDIATE)
            {
                enemySpawn.Spawn();
            }
        }

        current_level.hasStarted = true;
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
        else if (current_level.levelObjective == Level.LevelObjective.DEFEAT_BOSS)
        {
            isLevelCompleted = !current_level.cyberBoss;
        }

        current_level.hasStarted = !isLevelCompleted;
    }

    public static void Close()
    {
        if (current_level != null)
        {
            levelTimer = 0;
            enemyKilledCounter = 0;
            isLevelCompleted = false;
            Object.Destroy(current_level.gameObject);
        }
    }

    public static void StopAllEntites() 
    {
        foreach (Transform transform in current_level.EntityList.transform)
        {
            transform.gameObject.SetActive(false);
        }
    }

    /*
    TODO: Restart Level
    */
}

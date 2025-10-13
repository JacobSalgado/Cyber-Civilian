using System;
using UnityEngine;

public class Level : MonoBehaviour
{
    public enum LevelObjective
    {
        KILL_ALL_ENEMIES,
        ENEMY_COUNT,
        REACH_EXIT
    }

    [Header("==Necessary Level GameObjects==")]
    public GameObject EntityList;
    public GameObject EnemySpawns;
    public PolygonCollider2D confiner;
    public Player player;

    [Header("==Level Properties==")]
    public LevelObjective levelObjective;
    public int enemyKilledGoal;
    [NonSerialized] public bool isExitReached = false;
    
    void Start()
    {
        /* Clean up current Level's starting parameters */

        // If objective is to kill all enemies, no enemy spawn should be repeatable
        if (levelObjective == LevelObjective.KILL_ALL_ENEMIES)
        {
            foreach (Transform child in EnemySpawns.transform)
            {
                EnemySpawn enemySpawn = child.gameObject.GetComponent<EnemySpawn>();
                enemySpawn.repeatableSpawn = false;
            }
        }

        if (enemyKilledGoal < 0) enemyKilledGoal = 0;
    }

    // TODO: EXPAND ON LEVEL SCRIPT
}
using System;
using UnityEngine;

public class Level : MonoBehaviour
{
    public enum LevelObjective
    {
        KILL_ALL_ENEMIES,
        ENEMY_COUNT,
        REACH_EXIT,
        DEFEAT_BOSS,
        TUTORIAL,
    }

    [Header("==Necessary Level GameObjects==")]
    public GameObject EntityList;
    public GameObject EnemySpawns;
    public PolygonCollider2D confiner;
    public Player player;
    public AudioManager audioManager;
    [SerializeField] private AudioEffect[] SFX;

    [Header("==Level Properties==")]
    public LevelObjective levelObjective;
    public int enemyKilledGoal;
    [NonSerialized] public bool hasStarted = false;
    [NonSerialized] public bool isExitReached = false;
    [NonSerialized] public GameObject cyberBoss = null;
    
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
        else if (levelObjective == LevelObjective.DEFEAT_BOSS)
        {
            cyberBoss = EntityList.transform.Find("CyberBoss").gameObject;
        }

        if (enemyKilledGoal < 0) enemyKilledGoal = 0;

        //audioManager.InitializeAudioDictionary(SFX);
    }

    // TODO: EXPAND ON LEVEL SCRIPT
}
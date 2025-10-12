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
    public readonly LevelObjective levelObjective;

    void Update()
    {
        if (levelObjective == LevelObjective.KILL_ALL_ENEMIES)
        {
            LevelManager.isLevelCompleted = LevelManager.enemyKilledCounter >= LevelManager.enemyKilledGoal;
        }
    }

    public void LevelClose()
    {
        Destroy(gameObject);
    }

    // TODO: EXPAND ON LEVEL SCRIPT
}
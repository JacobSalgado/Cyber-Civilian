using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Windows.Speech;

public class EnemySpawn : MonoBehaviour
{
    public enum EnemySpawnCondition
    {
        IMMEDIATE,
        TIMED,
    }

    [Header("==Necessary GameObjects==")]
    public GameObject enemyPrefab;
    public EnemySpawnCondition spawnCondition = EnemySpawnCondition.IMMEDIATE;
    public bool repeatableSpawn = false;
    public float timeTilRespawn; // only checked if repeatableSpawn is true
    //public Dictionary<string, int> enemySpawnLimit = new();

    // Non-Serialized Vars
    [NonSerialized] public float respawnTimer = 0.0f;
    [NonSerialized] public bool isSpawned = false;
    private Enemy enemy;

    void Update()
    {
        isSpawned = repeatableSpawn && enemy != null;

        if (repeatableSpawn && !isSpawned)
        {
            respawnTimer += Time.deltaTime;
            if (respawnTimer > timeTilRespawn)
            {
                Spawn();
                respawnTimer = 0.0f;
            }
        }
    }
    
    public void Spawn()
    {
        if (enemyPrefab == null)
        {
            Debug.LogError("No enemy assigned");
            return;
        }

        if (spawnCondition == EnemySpawnCondition.TIMED)
        {
            // TODO: check conditions here
        }

        enemy = Instantiate(enemyPrefab, transform.position, transform.rotation, LevelManager.current_level.EntityList.transform).GetComponent<Enemy>();

        isSpawned = true;
        respawnTimer = 0;
    }
}

using System;
using System.Collections;
using UnityEngine;

public class EnemySpawn : MonoBehaviour
{
    public enum EnemySpawnCondition
    {
        IMMEDIATE,
        TIMED,
    }

    [Header("==Necessary GameObjects==")]
    public Enemy.EnemyTypes typeToSpawn;
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
                StartCoroutine(_Spawn());
                respawnTimer = 0.0f;
            }
        }
    }

    public void Spawn()
    {
        StartCoroutine(_Spawn());
    }

    private IEnumerator _Spawn()
    {
        string enemyPrefabPath = "Entities/Enemy/";

        // TODO: change enemy names
        string enemyName = typeToSpawn switch
        {
            Enemy.EnemyTypes.SHARK => "Trooper",
            Enemy.EnemyTypes.CRAB => "Sniper",
            Enemy.EnemyTypes.MANTIS => "Mantis",
            Enemy.EnemyTypes.TRAPPER => "Trapper",
            Enemy.EnemyTypes.HOMING => "Homing",
            Enemy.EnemyTypes.CYBERBOSS => "CyberBoss",
            _ => "",
        };
        enemyPrefabPath += enemyName + "/" + enemyName;

        //Debug.Log(enemyPrefabPath);

        if (spawnCondition == EnemySpawnCondition.TIMED)
        {
            // TODO: check conditions here
        }

        ResourceRequest request = Resources.LoadAsync<GameObject>(enemyPrefabPath);
        while (!request.isDone)
        {
            //Debug.Log("loading game over prefab");
            yield return null;
        }

        yield return StartCoroutine(SpawnEnemy(request.asset as GameObject));

        isSpawned = true;
        respawnTimer = 0;
    }
    
    private IEnumerator SpawnEnemy(GameObject prefab)
    {
        enemy = Instantiate(prefab, transform.position, transform.rotation, LevelManager.current_level.EntityList.transform).GetComponent<Enemy>();
        yield return null;
    }
}

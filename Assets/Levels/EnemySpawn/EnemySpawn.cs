using UnityEditor;
using UnityEngine;

public class EnemySpawn : MonoBehaviour
{
    public enum EnemySpawnCondition
    {
        IMMEDIATE,
        TIMED,
    }

    public GameObject enemyPrefab;
    public bool repeatable = false;
    
    public void Spawn()
    {
        if (enemyPrefab == null)
        {
            Debug.LogError("No enemy assigned");
            return;
        }

        //GameObject instance = (GameObject)PrefabUtility.InstantiatePrefab(enemyPrefab);
        
        Instantiate(enemyPrefab, transform.position, transform.rotation, LevelManager.current_level.EntityList.transform);
    }
}

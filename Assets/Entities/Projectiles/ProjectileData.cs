using UnityEngine;

// data held by weapons to influence the projectile they shoot
[CreateAssetMenu(fileName = "ProjectileData", menuName = "Scriptable Objects/ProjectileData")]
public class ProjectileData : EntityData
{
    [Header("==General Projectile Data==")]
    public int damage;
    public float lifeTime;
    public float timeToSpawn;
    public GameObject hitEffect;
}

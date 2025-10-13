using UnityEngine;

// data held by weapons to influence the projectile they shoot
[CreateAssetMenu(fileName = "ProjectileData", menuName = "Scriptable Objects/ProjectileData")]
public class ProjectileData : EntityData
{
    [Header("==General Projectile Data==")]
    public int damage;
    public GameObject hitEffect;

    [Header("==Optional Projectile Data==")]
    public float lifeTime;
    public float timeToSpawn;
    public bool destroyOnCollision = true;

    [Header("==Railshot Properties==")]
    public float railshotLength = 10f;
    public float fadeawayTime = 1f;
}

using UnityEngine;

[CreateAssetMenu(fileName = "EntityData", menuName = "Scriptable Objects/EntityData")]
public class EntityData : ScriptableObject
{
    // General Entity Properties
    public int currentHealth;
    public int maxHealth;
    public float moveSpeed;
    public AudioClip[] SFX;
}

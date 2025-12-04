using UnityEngine;

[CreateAssetMenu(fileName = "EntityData", menuName = "Scriptable Objects/EntityData")]
public abstract class EntityData : ScriptableObject
{
    /*
    * EntityData Scriptable Object:
    * - General data variables that entities may or may not need
    * - Contains numeric data or data related to the entity's behavior
    * - Can be accessed and changed directly (i.e. doesn't need the owner to access it)
    */

    [Header("==General Entity Properties==")]
    public int currentHealth;
    public int maxHealth;
    public float moveSpeed;

    [Header("==Enemy Drop Chances==")]
    public int medpackDropChance;
}

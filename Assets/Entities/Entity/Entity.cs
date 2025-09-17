using UnityEngine;

public abstract class Entity : StateManager
{
    public EntityData entityData;
    public Rigidbody2D rb;
    public AudioSource SFXPlayer;

    public abstract void InitializeStates();
}

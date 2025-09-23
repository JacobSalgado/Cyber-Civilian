using JetBrains.Annotations;
using UnityEngine;

public abstract class Entity : StateManager
{
    public EntityData entityData;
    public Rigidbody2D rb;
    public AudioSource SFXPlayer;

    public bool invincibility = false;

    public abstract void InitializeStates();

    public void TakeDamage(int damageTaken)
    {
        if (invincibility) return;

        if (entityData.currentHealth - damageTaken <= 0)
        {
            entityData.currentHealth = 0;
            this.ChangeState("Death");
        }
        else
            entityData.currentHealth -= damageTaken;
    }
}

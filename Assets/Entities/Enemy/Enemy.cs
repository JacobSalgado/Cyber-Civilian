using UnityEngine;

public class Enemy : Entity
{
    public Transform target; // following the player

    public override void InitializeStates()
    {
        AddState("Idle", new EnemyIdle(this));
        AddState("Move", new EnemyMove(this));
        current_state = stateMap["Idle"];
    }

    private void Start()
    {
        InitializeStates();
    }

    public override void FixedUpdate()
    {
        base.FixedUpdate();
    }
}

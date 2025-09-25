using UnityEngine.InputSystem;

public class Player : Entity
{
    public InputActionReference moveAction;

    public override void InitializeStates()
    {
        AddState("Idle", new PlayerIdle(this));
        AddState("Move", new PlayerMove(this));
        AddState("Death", new EntityDeath(this));

        ChangeState("Idle");
    }

    void Start()
    {
        InitializeStates();
        //healthBar = new HealthBar(entityData, );
    }

    void FixedUpdate()
    {
        current_state.UpdateState();
    }
}

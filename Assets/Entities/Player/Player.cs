using UnityEngine.InputSystem;

public class Player : Entity
{
    public InputActionReference moveAction;

    public override void InitializeStates()
    {
        AddState("Idle", new PlayerIdle(this));
        AddState("Move", new PlayerMove(this));
        current_state = stateMap["Idle"];
    }

    void Start()
    {
        InitializeStates();
    }

    void FixedUpdate()
    {
        current_state.UpdateState();
    }
}

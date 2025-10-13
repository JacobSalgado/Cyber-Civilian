using System.Collections.Generic;

public class GamePauseMenu : State
{
    readonly GameManager manager;

    public GamePauseMenu(GameManager gameManager) : base(gameManager)
    {
        manager = gameManager;
    }

    public override void EnterState(Dictionary<string, object> args = null)
    {
        // TODO: load the menu prefab and attach it to UI Holder

        // TODO: assign listeners for buttons

        // TODO: pause all entities
    }

    public override void UpdateState()
    {
        
    }

    public override void ExitState(Dictionary<string, object> args = null)
    {

    }
}

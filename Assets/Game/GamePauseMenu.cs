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

    }

    public override void UpdateState()
    {

    }

    public override void ExitState(Dictionary<string, object> args = null)
    {

    }
}

using System.Collections.Generic;
using UnityEditor;

public class GameMainMenu : State
{
    readonly GameManager manager;

    public GameMainMenu(GameManager gameManager) : base(gameManager)
    {
        manager = gameManager;
    }

    public override void EnterState(Dictionary<string, object> args = null)
    {
        manager.cursor.SetCursorType(GameCursor.CursorType.POINTER);
        manager.UIHolder.SetActive(true);
    }

    public override void UpdateState()
    {
        
    }

    public override void ExitState(Dictionary<string, object> args = null)
    {
        manager.mainMenu.MainMenuClose();
        manager.mainMenu = null;
    }
}

using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class GameMainMenu : State
{
    readonly GameManager manager;
    const string menu_path = "Assets/UI/MainMenu/MainMenu.prefab";
    private MainMenu menu;

    public GameMainMenu(GameManager gameManager) : base(gameManager)
    {
        manager = gameManager;
    }

    public override void EnterState(Dictionary<string, object> args = null)
    {
        // load the menu prefab and attach it to UI Holder
        menu = PrefabUtility.LoadPrefabContents(menu_path).GetComponent<MainMenu>();
        menu.gameObject.transform.SetParent(manager.UIHolder.transform, false);

        // assign listeners for buttons
        menu.buttons[0].onClick.AddListener(manager.StartGameButton);
        menu.buttons[1].onClick.AddListener(manager.EndGameButton);
    }

    public override void UpdateState()
    {
        
    }

    public override void ExitState(Dictionary<string, object> args = null)
    {
        menu.MainMenuClose();
    }
}

using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class GamePauseMenu : State
{
    readonly GameManager manager;
    const string menu_path = "Assets/UI/PauseMenu/PauseMenu.prefab";
    private PauseMenu menu;

    public GamePauseMenu(GameManager gameManager) : base(gameManager)
    {
        manager = gameManager;
    }

    public override void EnterState(Dictionary<string, object> args = null)
    {
        // load the menu prefab and attach it to UI Holder
        menu = PrefabUtility.LoadPrefabContents(menu_path).GetComponent<PauseMenu>();
        menu.gameObject.transform.SetParent(manager.UIHolder.transform, false);

        // assign listeners for buttons
        menu.buttons[0].onClick.AddListener(manager.ResumeGameButton);
        menu.buttons[1].onClick.AddListener(manager.ExitGameButton);

        // hide LevelHolder
        //manager.levelHolder.

        Time.timeScale = 0f;
    }

    public override void UpdateState()
    {
        
    }

    public override void ExitState(Dictionary<string, object> args = null)
    {
        menu.PauseMenuClose();

        Time.timeScale = 1f;
    }
}

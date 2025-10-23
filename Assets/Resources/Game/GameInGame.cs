using System.Collections.Generic;
using UnityEngine;

public class GameInGame : State
{
    const string playerHUDPath = "Assets/UI/PlayerHUD/PlayerHUD.prefab";
    readonly GameManager manager;

    public GameInGame(GameManager gameManager) : base(gameManager)
    {
        manager = gameManager;
    }

    public override void EnterState(Dictionary<string, object> args = null)
    {
        manager.camera.gameObject.SetActive(true);
        LevelManager.StartLevel();
        manager.UIHolder.SetActive(true);
    }

    public override void UpdateState()
    {
        //LevelManager.Update();

        // check level completion
        if (LevelManager.isLevelCompleted)
        {
            manager.levelIndex++;
            if (manager.levelIndex >= manager.levelList.Length) // Game Complete
            {
                manager.ChangeState("LoadingScreen", new Dictionary<string, object>()
                {
                    {"nextState", "MainMenu"}
                });
                return;
                
            }
            else // Next Level
            {
                manager.ChangeState("LoadingScreen", new Dictionary<string, object>()
                {
                    {"nextState", "InGame"},
                    {"UpdatePlayer", true}
                });
            }
        }
    }

    public override void ExitState(Dictionary<string, object> args = null)
    {
        // close current level
        LevelManager.Close();

        // close HUD
        manager.hud.PlayerHUDClose();
        manager.hud = null;
    }
}

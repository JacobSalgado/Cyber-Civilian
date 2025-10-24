using System.Collections.Generic;

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
        manager.UIHolder.SetActive(true);

        if (args == null || (args != null && !args.ContainsKey("FromPauseMenu")))
        {
            manager.audioManager.PlayAudioSource("Level1");
            LevelManager.StartLevel();
        }

    }

    public override void UpdateState()
    {
        if (manager.pauseAction.action.WasPressedThisFrame())
        {
            manager.levelHolder.SetActive(false);
            manager.UIHolder.SetActive(false);
            manager.ChangeState("PauseMenu");
            return;
        }

        LevelManager.Update();

        // check level completion
        if (LevelManager.isLevelCompleted)
        {
            manager.levelIndex++;
            if (manager.levelIndex >= manager.levelList.Length) // Game Complete
            {
                manager.ChangeState("LoadingScreen", new Dictionary<string, object>()
                {
                    {"nextState", GameManager.GameState.MAIN_MENU}
                });
                return;

            }
            else // Next Level
            {
                manager.ChangeState("LoadingScreen", new Dictionary<string, object>()
                {
                    {"nextState", GameManager.GameState.IN_GAME},
                    {"UpdatePlayer", true},
                    {"currentPlayerHealth", manager.player.playerData.currentHealth},
                    {"currentPlayerMaxHealth", manager.player.playerData.maxHealth},
                });
            }
        }
    }

    public override void ExitState(Dictionary<string, object> args = null)
    {
        // close current level
        if (args != null)
        {
            manager.hud.PlayerHUDClose();
            manager.hud = null;

            LevelManager.Close();
        }
    }
}

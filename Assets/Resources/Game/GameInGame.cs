using System.Collections.Generic;
using UnityEngine;

public class GameInGame : State
{
    readonly GameManager manager;

    public GameInGame(GameManager gameManager) : base(gameManager)
    {
        manager = gameManager;
    }

    public override void EnterState(Dictionary<string, object> args = null)
    {
        manager.UIHolder.SetActive(true);

        manager.cursor.SetCursorType(GameCursor.CursorType.RETICLE);

        if (args == null || (args != null && !args.ContainsKey("FromPauseMenu")))
        {
            manager.audioManager.PlayAudioSource("Level1"); // TODO: ACCOMODATE FOR MULTIPLE LEVEL BGMS
            LevelManager.StartLevel();
        }
    }

    public override void UpdateState()
    {
        if (manager.pauseAction.action.WasPressedThisFrame())
        {
            manager.levelHolder.SetActive(false);
            manager.UIHolder.SetActive(false);
            manager.audioManager.PlayAudioSource("Pause");
            manager.ChangeState("PauseMenu");
            return;
        }

        LevelManager.Update();

        // make listener game object follow player
        manager.listener.gameObject.transform.position = LevelManager.player.gameObject.transform.position;

        // check if player is dead
        if (manager.player.isDead)
        {
            manager.ChangeState("LoadingScreen", new Dictionary<string, object>()
            {
                {"nextState", GameManager.GameState.GAME_OVER}
            });
            return;
        }

        // check level completion
        if (LevelManager.isLevelCompleted)
        {
            manager.levelIndex++;
            manager.audioManager.StopAudioSource("Level1");
            LevelManager.StopAllEntites();
    
            if (manager.levelIndex >= manager.levelList.Length || LevelManager.current_level is Tutorial) // Game Complete
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
        manager.listener.gameObject.transform.position = Vector2.zero;
    }
}

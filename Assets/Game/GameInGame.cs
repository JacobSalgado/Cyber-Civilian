using System.Collections.Generic;
using Unity.Cinemachine;
using UnityEditor;

public class GameInGame : State
{
    const string playerHUDPath = "Assets/UI/PlayerHUD/PlayerHUD.prefab";
    readonly GameManager manager;
    private PlayerHUD hud;

    public GameInGame(GameManager gameManager) : base(gameManager)
    {
        manager = gameManager;
    }

    public override void EnterState(Dictionary<string, object> args = null)
    {
        Level new_level;

        if (args != null && args.ContainsKey("FromPauseMenu"))
        {
            // TODO: resume normal game operation
            return;
        }
        else LevelManager.LoadLevel(manager.levelList[manager.levelIndex], manager.levelHolder); // initialize selected level
        new_level = LevelManager.current_level;

        manager.player = new_level.player;
        if (args != null && args.ContainsKey("UpdatePlayer"))
        {
            // TODO: update base player prefab to match state from previous level
        }

        // load PlayerHUD prefab
        hud = PrefabUtility.LoadPrefabContents(playerHUDPath).GetComponent<PlayerHUD>();
        hud.gameObject.transform.SetParent(manager.UIHolder.transform, false);

        manager.player.healthBar = hud.healthSlider;
        manager.player.UpdateHealthBar();

        manager.player.resourceMeter = hud.resourceSlider;
        manager.player.UpdateResourceMeter();

        // connect current level's camera confiner and player object to the game camera
        manager.cinemachine.GetComponent<CinemachineConfiner2D>().BoundingShape2D = new_level.confiner;
        manager.cinemachine.Follow = manager.player.transform;

        // connect player to cam for mouse aiming
        manager.player.cam = manager.camera;
    }

    public override void UpdateState()
    {
        LevelManager.Update();

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
        hud.PlayerHUDClose();
    }
}

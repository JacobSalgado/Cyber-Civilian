using System.Collections.Generic;
using Unity.Cinemachine;
using UnityEditor;
using UnityEngine;

public class GameInGame : State
{
    const string playerHUDPath= "Assets/UI/PlayerHUD/PlayerHUD.prefab";
    readonly GameManager manager;
    private PlayerHUD hud;

    public GameInGame(GameManager gameManager) : base(gameManager)
    {
        manager = gameManager;
    }

    public override void EnterState(Dictionary<string, object> args = null)
    {
        Level new_level;

        LevelManager.LoadLevel(manager.levelList[1], manager.levelHolder);
        new_level = LevelManager.current_level;

        manager.player = new_level.player;

        // load PlayerHUD prefab
        hud = PrefabUtility.LoadPrefabContents(playerHUDPath).GetComponent<PlayerHUD>();
        hud.gameObject.transform.SetParent(manager.UIHolder.transform, false);

        manager.player.healthBar = hud.healthSlider.GetComponent<HealthBar>();
        manager.player.healthBar.UpdateHealthBar();

        // connect current level's camera confiner and player object to the game camera
        manager.cinemachine.GetComponent<CinemachineConfiner2D>().BoundingShape2D = new_level.confiner;
        manager.cinemachine.Follow = manager.player.transform;

        // connect player to cam for mouse aiming
        manager.player.cam = manager.camera;
    }

    public override void UpdateState()
    {

    }

    public override void ExitState(Dictionary<string, object> args = null)
    {
        // close current level
        LevelManager.Close();

        // close HUD
        hud.PlayerHUDClose();
    }
}

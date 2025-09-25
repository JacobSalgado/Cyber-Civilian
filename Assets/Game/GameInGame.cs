using System.Collections.Generic;
using Unity.Cinemachine;
using UnityEditor;
using UnityEngine;

public class GameInGame : State
{
    //const string playerHUDPath= "";
    readonly GameManager manager;

    public GameInGame(GameManager gameManager) : base(gameManager)
    {
        manager = gameManager;
    }

    public override void EnterState(Dictionary<string, object> args = null)
    {
        manager.levelManager.LoadLevel(manager.levelList[0]);
        manager.player = manager.levelManager.current_level.player;

        // load PlayerHUD prefab
        //PrefabUtility.LoadPrefabContents(playerHUDPath);
        //.transform.SetParent(manager.transform, false);
        //manager.UIHolder

        // connect current level's camera confiner and player object to the game camera
        manager.cinemachine.GetComponent<CinemachineConfiner2D>().BoundingShape2D = manager.levelManager.current_level.confiner;
        manager.cinemachine.Follow = manager.player.transform;

        manager.player.cam = manager.camera;
    }

    public override void UpdateState()
    {

    }
    
    public override void ExitState(Dictionary<string, object> args = null)
    {
        
    }
}

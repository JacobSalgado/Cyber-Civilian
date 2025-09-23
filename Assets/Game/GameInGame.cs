using System.Collections.Generic;
using UnityEngine;

public class GameInGame : State
{
    GameManager manager;

    public GameInGame(GameManager gameManager) : base(gameManager)
    {
        manager = gameManager;
    }

    public override void EnterState(Dictionary<string, object> args = null)
    {
        manager.levelManager.LoadLevel(manager.level_list[0]);

        // load ui object
        
        //.transform.SetParent(manager.transform, false);
        //manager.UIHolder
    }

    public override void ExitState(Dictionary<string, object> args = null)
    {
        
    }

    public override void UpdateState()
    {
        
    }
}

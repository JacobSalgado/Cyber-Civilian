using System;
using System.Collections.Generic;
using UnityEngine;

public class GameLoadingScreen :  State
{
    readonly GameManager manager;
    string nextState = string.Empty;
    private float loadingTimer = 0;

    public GameLoadingScreen(GameManager gameManager) : base(gameManager)
    {
        manager = gameManager;
    }

    public override void EnterState(Dictionary<string, object> args = null)
    {
        // TODO: load the menu prefab and attach it to UI Holder

        // get next state
        if (args != null && args.ContainsKey("nextState"))
            nextState = (string) args["nextState"];
    }

    public override void UpdateState()
    {
        loadingTimer += Time.deltaTime;

        if (loadingTimer >= manager.loadingTime)
            manager.ChangeState(nextState);
    }

    public override void ExitState(Dictionary<string, object> args = null)
    {
        loadingTimer = 0;
        nextState = string.Empty;
    }
}

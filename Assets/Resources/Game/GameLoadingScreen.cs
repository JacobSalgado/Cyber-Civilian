using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.Threading.Tasks;
using Unity.Cinemachine;

public class GameLoadingScreen : State
{
    readonly GameManager manager;
    GameManager.GameState nextState;
    Dictionary<string, object> prev_args = null;
    // bool isRequestDone = false;
    // private float loadingTimer = 0;


    public GameLoadingScreen(GameManager gameManager) : base(gameManager)
    {
        manager = gameManager;
    }

    public override void EnterState(Dictionary<string, object> args = null)
    {
        // TODO: load the menu prefab and attach it to UI Holder

        // get next state
        if (args != null && args.ContainsKey("nextState"))
        {
            nextState = (GameManager.GameState)args["nextState"];
            prev_args = args;
        }

        manager.isLoading = true;
        switch (nextState)
        {
            case GameManager.GameState.MAIN_MENU:
                manager.LoadMainMenuAsync();

                break;

            case GameManager.GameState.IN_GAME:
                manager.LoadInGameAsync();

                break;
        }
    }

    public override void UpdateState()
    {
        /*
        loadingTimer += Time.deltaTime;

        if (loadingTimer >= manager.loadingTime)
            manager.ChangeState(nextState);
        */
        if (!manager.isLoading)
            manager.ChangeState(manager.GameState_To_String(nextState), prev_args);
    }

    public override void ExitState(Dictionary<string, object> args = null)
    {
        // loadingTimer = 0;
        // prev_args = null;
    }
}

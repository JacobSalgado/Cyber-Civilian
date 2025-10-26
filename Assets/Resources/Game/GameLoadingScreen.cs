using System.Collections.Generic;

public class GameLoadingScreen : State
{
    readonly GameManager manager;
    GameManager.GameState nextState;
    Dictionary<string, object> prev_args = null;

    public GameLoadingScreen(GameManager gameManager) : base(gameManager)
    {
        manager = gameManager;
    }

    public override void EnterState(Dictionary<string, object> args = null)
    {
        prev_args = null;

        manager.UIHolder.SetActive(false);
        manager.loadingScreenHolder.SetActive(true);
        manager.loadingProgress = 0f;
        manager.loadingScreenSlider.value = 0f;

        // get next state
        if (args != null && args.ContainsKey("nextState"))
        {
            nextState = (GameManager.GameState) args["nextState"];
            prev_args = args;
            manager.args = args;
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

            case GameManager.GameState.GAME_OVER:
                manager.LoadGameOverMenuAsync();

                break;
        }
    }

    public override void UpdateState()
    {
        manager.loadingScreenSlider.value = manager.loadingProgress;

        if (!manager.isLoading)
            manager.ChangeState(manager.GameState_To_String(nextState), prev_args);
    }

    public override void ExitState(Dictionary<string, object> args = null)
    {
        if (nextState == GameManager.GameState.IN_GAME)
        {
            manager.camera.gameObject.SetActive(true);
        }
        manager.loadingScreenHolder.SetActive(false);
    }
}

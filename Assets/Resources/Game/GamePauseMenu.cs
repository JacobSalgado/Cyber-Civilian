using System.Collections.Generic;
using UnityEngine;

public class GamePauseMenu : State
{
    readonly GameManager manager;

    public GamePauseMenu(GameManager gameManager) : base(gameManager)
    {
        manager = gameManager;
    }

    public override void EnterState(Dictionary<string, object> args = null)
    {
        manager.pauseMenu.gameObject.SetActive(true);
        Time.timeScale = 0f;
    }

    public override void UpdateState()
    {
        if (manager.pauseAction.action.WasPressedThisFrame())
        {
            manager.ChangeState("InGame", new Dictionary<string, object>()
            {
                {"FromPauseMenu", true}
            });
            return;
        }
    }

    public override void ExitState(Dictionary<string, object> args = null)
    {
        manager.pauseMenu.gameObject.SetActive(false);
        manager.levelHolder.SetActive(true);
        Time.timeScale = 1f;
    }
}

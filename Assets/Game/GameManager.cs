using System;
using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.InputSystem;

public class GameManager : StateManager
{
    public enum GameState
    {
        MAIN_MENU,
        PAUSE_MENU,
        IN_GAME
    }

    [Header("==Necessary GameManager Objects==")]
    public GameObject UIHolder;
    public AudioSource BGMPlayer;
    public GameObject levelHolder;
    public new Camera camera;
    public CinemachineCamera cinemachine;
    public InputActionReference[] uiActions;

    [Header("==Game Parameters==")]
    public string[] levelList;
    [SerializeField] private GameState startingState;

    /* Non-Serialized Vars */
    [NonSerialized] public Player player;

    void Start()
    {
        // initialize states
        AddState("MainMenu", new GameMainMenu(this));
        AddState("InGame", new GameInGame(this));

        ChangeState(GameState_To_String(startingState));
    }

    void FixedUpdate()
    {
        current_state.UpdateState();
    }

    public void StartGame()
    {
        ChangeState("InGame");
    }

    public void EndGame()
    {
        // TODO: implement end game
        Debug.Log("Game Ended");
    }

    private string GameState_To_String(GameState state)
    {
        string state_key;

        switch (state)
        {
            case GameState.MAIN_MENU:
                state_key = "MainMenu";
                break;

            case GameState.PAUSE_MENU:
                state_key = "PauseMenu";
                break;

            default:
                state_key = "InGame";
                break;
        }

        return state_key;
    }
}
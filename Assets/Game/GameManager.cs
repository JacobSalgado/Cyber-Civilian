using System;
using System.Collections.Generic;
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
    //public InputActionReference[] uiActions;
    public AudioManager audioManager;

    [Header("==Game Parameters==")]
    public string[] levelList;
    [SerializeField] private GameState startingState;
    public float loadingTime = 3;

    [Header("==BGM==")]
    [SerializeField] private string[] BGMNames;
    [SerializeField] private AudioClip[] BGM;

    /* Non-Serialized Vars */
    [NonSerialized] public Player player;
    [NonSerialized] public int levelIndex = 0;

    void Start()
    {
        // initialize states
        AddState("MainMenu", new GameMainMenu(this));
        AddState("InGame", new GameInGame(this));
        AddState("LoadingScreen", new GameLoadingScreen(this));

        ChangeState(GameState_To_String(startingState));

        // initialize audio manager
        audioManager.InitializeAudioDictionary(BGMNames, BGM);
    }

    void Update()
    {
        current_state.UpdateState();
    }

    public void StartGameButton()
    {
        levelIndex = 0;
        ChangeState("LoadingScreen", new Dictionary<string, object>()
        {
            {"nextState", "InGame"}
        });
    }

    public void EndGameButton()
    {
        // TODO: implement end game
        Debug.Log("Game Ended");
    }

    private string GameState_To_String(GameState state)
    {
        string state_key = state switch
        {
            GameState.MAIN_MENU => "MainMenu",
            GameState.PAUSE_MENU => "PauseMenu",
            _ => "InGame",
        };
        return state_key;
    }
}
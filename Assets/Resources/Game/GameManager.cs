using System;
using System.Collections;
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
    public InputActionReference pauseAction;
    public AudioManager audioManager;

    [Header("==Game Parameters==")]
    public string[] levelList;
    [SerializeField] private GameState startingState;
    public float loadingTime = 3;

    [Header("==BGM==")]
    [SerializeField] private string[] BGMNames;
    [SerializeField] private AudioClip[] BGM;

    /* Non-Serialized Vars */
    [NonSerialized] public bool isLoading = false;
    [NonSerialized] public Player player;
    [NonSerialized] public int levelIndex = 0;
    const string mainMenuPath = "UI/MainMenu/MainMenu";
    const string playerHUDPath = "UI/PlayerHUD/PlayerHUD";

    public MainMenu mainMenu = null;
    public PlayerHUD hud = null;
    public PauseMenu pauseMenu = null;
    public Dictionary<string, object> args = new();

    void Start()
    {
        // initialize states
        AddState("MainMenu", new GameMainMenu(this));
        AddState("InGame", new GameInGame(this));
        AddState("LoadingScreen", new GameLoadingScreen(this));

        ChangeState("LoadingScreen", new Dictionary<string, object>()
        {
            { "nextState", startingState }
        });

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
            { "nextState", GameState.IN_GAME }
        });
    }

    public void EndGameButton()
    {
        // TODO: implement end game
        Debug.Log("Game Ended");
    }

    public void ResumeGameButton()
    {
        ChangeState("LoadingScreen", new Dictionary<string, object>()
        {
            {"nextState", GameState.IN_GAME},
            {"FromPauseMenu", true},
        });
    }

    public void ExitGameButton()
    {
        ChangeState("LoadingScreen", new Dictionary<string, object>()
        {
            {"nextState", "MainMenu"}
        });
    }

    public string GameState_To_String(GameState state)
    {
        string state_key = state switch
        {
            GameState.MAIN_MENU => "MainMenu",
            GameState.PAUSE_MENU => "PauseMenu",
            _ => "InGame",
        };
        return state_key;
    }

    public void LoadMainMenuAsync()
    {
        UIHolder.SetActive(false);
        StartCoroutine(LoadMainMenu());
    }

    private IEnumerator LoadMainMenu()
    {
        // load the menu prefab and attach it to UI Holder
        ResourceRequest request = Resources.LoadAsync<GameObject>(mainMenuPath);
        while (!request.isDone)
        {
            Debug.Log("loading main menu prefab");
            yield return null;
        }

        // assign listeners for buttons
        yield return StartCoroutine(InitializeMainMenu(request.asset as GameObject));

        isLoading = false;
    }

    private IEnumerator InitializeMainMenu(GameObject prefab)
    {
        GameObject instance = Instantiate(prefab, UIHolder.transform);
        mainMenu = instance.GetComponent<MainMenu>();

        mainMenu.buttons[0].onClick.AddListener(StartGameButton);
        mainMenu.buttons[1].onClick.AddListener(EndGameButton);

        yield return null;
    }

    public void LoadInGameAsync()
    {
        UIHolder.SetActive(false);
        StartCoroutine(LoadInGame());
    }

    private IEnumerator LoadInGame()
    {
        Level new_level;

        yield return LevelManager.LoadLevel(levelList[levelIndex], this);
        new_level = LevelManager.current_level;

        player = new_level.player;
        if (args != null && args.ContainsKey("UpdatePlayer"))
        {
            // TODO: update base player prefab to match state from previous level
        }

        // load PlayerHUD prefab
        ResourceRequest request = Resources.LoadAsync<GameObject>(playerHUDPath);
        while (!request.isDone)
        {
            Debug.Log("loading hud prefab prefab");
            yield return null;
        }

        yield return StartCoroutine(InitializePlayerHUD(request.asset as GameObject));

        yield return StartCoroutine(InitializeGameCamera(new_level));

        isLoading = false;
    }

    private IEnumerator InitializePlayerHUD(GameObject prefab)
    {
        GameObject instance = Instantiate(prefab, UIHolder.transform);
        hud = instance.GetComponent<PlayerHUD>();

        player.healthBar = hud.healthSlider;
        player.UpdateHealthBar();

        player.resourceMeter = hud.resourceSlider;
        player.UpdateResourceMeter();

        yield return null;
    }

    private IEnumerator InitializeGameCamera(Level new_level)
    {
        // connect current level's camera confiner and player object to the game camera
        cinemachine.GetComponent<CinemachineConfiner2D>().BoundingShape2D = new_level.confiner;
        cinemachine.Follow = player.transform;

        // connect player to cam for mouse aiming
        player.cam = camera;

        camera.gameObject.SetActive(false);

        yield return null;
    }
    
    public GameObject InstantiatePrefab(GameObject asset, GameObject parent)
    {
        return Instantiate(asset, parent.transform);
    }
}
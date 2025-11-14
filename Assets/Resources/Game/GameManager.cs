using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class GameManager : StateManager
{
    public enum GameState
    {
        MAIN_MENU,
        PAUSE_MENU,
        IN_GAME,
        GAME_OVER
    }

    [Header("==Necessary GameManager Objects==")]
    public GameObject UIHolder;
    public GameObject levelHolder;
    public GameObject loadingScreenHolder;
    public Slider loadingScreenSlider;
    public new Camera camera;
    public CinemachineCamera cinemachine;
    public InputActionReference pauseAction;
    public AudioManager audioManager;
    public PauseMenu pauseMenu;

    [Header("==Game Parameters==")]
    public string[] levelList;
    [SerializeField] private GameState startingState;
    [SerializeField] private AudioEffect[] BGM;
    [SerializeField] private AudioEffect[] uiSFX;

    /* Non-Serialized Vars */
    [NonSerialized] public bool isLoading = false;
    [NonSerialized] public Player player;
    [NonSerialized] public int levelIndex = 0;
    [NonSerialized] public float loadingProgress = 0f;
    const string mainMenuPath = "UI/MainMenu/MainMenu";
    const string playerHUDPath = "UI/PlayerHUD/PlayerHUD";
    const string gameOverPath = "UI/GameOverMenu/GameOverMenu";

    [NonSerialized] public MainMenu mainMenu = null;
    [NonSerialized] public GameOverMenu gameOver = null;
    [NonSerialized] public PlayerHUD hud = null;
    [NonSerialized] public Dictionary<string, object> args = new();

    void Start()
    {
        loadingScreenHolder.SetActive(false);
        loadingScreenSlider.value = 0f;

        InitializePauseMenu();

        // initialize states
        AddState("MainMenu", new GameMainMenu(this));
        AddState("InGame", new GameInGame(this));
        AddState("PauseMenu", new GamePauseMenu(this));
        AddState("GameOver", new GameGameOver(this));
        AddState("LoadingScreen", new GameLoadingScreen(this));

        ChangeState("LoadingScreen", new Dictionary<string, object>()
        {
            { "nextState", startingState }
        });

        // initialize audio manager
        audioManager.InitializeAudioDictionary(BGM);
        audioManager.InitializeAudioDictionary(uiSFX);
    }

    void Update()
    {
        current_state.UpdateState();
    }

    public void StartGameButton()
    {
        levelIndex = 0;
        audioManager.PlayAudioSource("Select");
        ChangeState("LoadingScreen", new Dictionary<string, object>()
        {
            { "nextState", GameState.IN_GAME }
        });
    }

    public void EndGameButton()
    {
        Debug.Log("Game Ended");
        #if UNITY_STANDALONE
            Application.Quit();
        #endif
        #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
        #endif
    }

    public void ResumeGameButton()
    {
        audioManager.PlayAudioSource("Resume");
        ChangeState("InGame", new Dictionary<string, object>()
        {
            {"FromPauseMenu", true}
        });
    }

    public void ExitGameButton()
    {
        audioManager.PlayAudioSource("Cancel");

        if (hud != null)
        {
            hud.PlayerHUDClose();
            hud = null;
        }

        LevelManager.Close();

        audioManager.StopAudioSource("Level1");

        ChangeState("LoadingScreen", new Dictionary<string, object>()
        {
            {"nextState", GameState.MAIN_MENU}
        });
    }

    public void RestartGameButton()
    {
        audioManager.PlayAudioSource("Select");
        audioManager.StopAudioSource("Level1");

        StartGameButton();
    }

    public string GameState_To_String(GameState state)
    {
        string state_key = state switch
        {
            GameState.MAIN_MENU => "MainMenu",
            GameState.PAUSE_MENU => "PauseMenu",
            GameState.GAME_OVER => "GameOver",
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
            loadingProgress = request.progress * 0.5f;
            yield return null;
        }

        loadingProgress = request.progress * 0.5f;

        // assign listeners for buttons
        yield return StartCoroutine(InitializeMainMenu(request.asset as GameObject));

        loadingProgress += 0.5f;

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


    public void LoadGameOverMenuAsync()
    {
        UIHolder.SetActive(false);
        StartCoroutine(LoadGameOverMenu());
    }

    private IEnumerator LoadGameOverMenu()
    {
        // load the menu prefab and attach it to UI Holder
        ResourceRequest request = Resources.LoadAsync<GameObject>(gameOverPath);
        while (!request.isDone)
        {
            Debug.Log("loading game over prefab");
            loadingProgress = request.progress * 0.5f;
            yield return null;
        }

        loadingProgress = request.progress * 0.5f;

        // assign listeners for buttons
        yield return StartCoroutine(InitializeGameOverMenu(request.asset as GameObject));

        loadingProgress += 0.5f;

        isLoading = false;
    }

    private IEnumerator InitializeGameOverMenu(GameObject prefab)
    {
        GameObject instance = Instantiate(prefab, UIHolder.transform);
        gameOver = instance.GetComponent<GameOverMenu>();

        gameOver.buttons[0].onClick.AddListener(RestartGameButton);
        gameOver.buttons[1].onClick.AddListener(ExitGameButton);

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

        loadingProgress = 0.25f;
        new_level = LevelManager.current_level;
        player = new_level.player;

        // load PlayerHUD prefab
        ResourceRequest request = Resources.LoadAsync<GameObject>(playerHUDPath);
        while (!request.isDone)
        {
            Debug.Log("loading hud prefab prefab");
            yield return null;

            loadingProgress = 0.25f + request.progress * 0.25f;
        }

        yield return StartCoroutine(InitializePlayerHUD(request.asset as GameObject));

        loadingProgress += 0.25f;

        yield return StartCoroutine(InitializeGameCamera(new_level));
        loadingProgress = 1.0f;

        isLoading = false;
    }

    private IEnumerator InitializePlayerHUD(GameObject prefab)
    {
        GameObject instance = Instantiate(prefab, UIHolder.transform);
        hud = instance.GetComponent<PlayerHUD>();

        if (args != null && args.ContainsKey("UpdatePlayer"))
        {
            // update base player prefab to match state from previous level
            player.updatePlayer = true;
            player.updateArgs = args;
        }

        player.healthBar = hud.healthSlider;
        player.UpdateHealthBar();

        player.resourceMeter = hud.resourceSlider;
        player.UpdateResourceMeter();

        player.ammoCount = hud.ammoCountText;
        player.UpdateAmmoCount();

        player.vortexMultiplier = hud.vortexMultiplierText;
        player.UpdateVortexMultiplier();

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

    private void InitializePauseMenu()
    {
        pauseMenu.gameObject.SetActive(false);

        pauseMenu.buttons[0].onClick.AddListener(ResumeGameButton);
        pauseMenu.buttons[1].onClick.AddListener(ExitGameButton);
    }
}
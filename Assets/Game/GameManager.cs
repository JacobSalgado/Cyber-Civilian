using System;
using Unity.Cinemachine;
using UnityEngine;

public class GameManager : StateManager
{
    [Header("Necessary GameManager Objects")]
    public GameObject UIHolder;
    public AudioSource BGMPlayer;
    public LevelManager levelManager;
    public new Camera camera;
    public CinemachineCamera cinemachine;

    [Header("Game Parameters")]
    public string[] levelList;
    [SerializeField] string startingState;

    /* Non-Serialized Vars */
    [NonSerialized] public Player player;

    void Start()
    {
        // initialize states
        AddState("MainMenu", new GameMainMenu(this));
        AddState("InGame", new GameInGame(this));

        ChangeState(startingState);
    }

    void FixedUpdate()
    {
        current_state.UpdateState();
    }
}
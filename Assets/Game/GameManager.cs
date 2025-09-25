using System;
using Unity.Cinemachine;
using Unity.VisualScripting;
using UnityEngine;

public class GameManager : StateManager
{
    [Header("Necessary GameManager Objects")]
    public GameObject UIHolder;
    public AudioSource BGMPlayer;
    public LevelManager levelManager;
    public CinemachineCamera cinemachine;

    [Header("Starting Parameters")]
    public string[] levelList;
    [SerializeField] string startingState;

    /* Non-Serialized Vars */
    [NonSerialized] public Player player;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // initialize states
        AddState("MainMenu", new GameMainMenu(this));
        AddState("InGame", new GameInGame(this));

        ChangeState(startingState);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        current_state.UpdateState();
    }
}
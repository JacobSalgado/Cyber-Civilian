using UnityEngine;

public class GameManager : StateManager
{
    [SerializeField]
    public LevelManager levelManager;

    [SerializeField]
    public AudioSource BGMPlayer;

    [SerializeField]
    public string[] level_list;

    [SerializeField]
    string startingState;

    [SerializeField]
    public GameObject UIHolder;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // initialize states
        AddState("InGame", new GameInGame(this));

        ChangeState(startingState);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        current_state.UpdateState();
    }
}
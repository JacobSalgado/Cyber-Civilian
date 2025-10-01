using UnityEngine;

public class Level : MonoBehaviour
{
    [Header("Necessary Level GameObjects")]
    public GameObject EntityList;
    public PolygonCollider2D confiner;
    public Player player;

    void Start()
    {
        // TODO: initialize stuff here
    }
}

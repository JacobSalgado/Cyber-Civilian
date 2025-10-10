using UnityEngine;

public class Level : MonoBehaviour
{
    [Header("Necessary Level GameObjects")]
    public GameObject EntityList;
    public PolygonCollider2D confiner;
    public Player player;

    public void LevelClose()
    {
        Destroy(gameObject);
    }

    // TODO: EXPAND ON LEVEL SCRIPT
}
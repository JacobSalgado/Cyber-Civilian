using UnityEngine;

public class GameCursor : MonoBehaviour
{
    public enum CursorType
    {
        RETICLE,
        POINTER,
    }

    public Texture2D pointerTexture;
    public Texture2D reticleTexture;

    public CursorType type;

    void Start()
    {
       SetCursorType(CursorType.POINTER);
    }

    // Update is called once per frame
    void Update()
    {
        if (type == CursorType.RETICLE)
        {
            
        }
    }

    public void SetCursorType(CursorType type)
    {
        this.type = type;
        switch (type)
        {
            case CursorType.POINTER:
                Cursor.SetCursor(pointerTexture, Vector2.zero, CursorMode.Auto);
                break;

            case CursorType.RETICLE:
                Cursor.SetCursor(reticleTexture, Vector2.zero, CursorMode.Auto);
                break;
        }
    }
}

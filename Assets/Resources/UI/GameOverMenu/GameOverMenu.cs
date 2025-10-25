using UnityEngine;
using UnityEngine.UI;

public class GameOverMenu : MonoBehaviour
{
    public Button[] buttons;

    public void GameOverMenuClose()
    {
        foreach (Button button in buttons)
        {
            button.onClick.RemoveAllListeners();
        }

        Destroy(gameObject);
    }
}

using UnityEngine;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    public Button[] buttons;

    public void MainMenuClose()
    {
        foreach (Button button in buttons)
        {
            button.onClick.RemoveAllListeners();
        }

        Destroy(gameObject);
    }
}

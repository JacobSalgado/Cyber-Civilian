using UnityEngine;
using UnityEngine.UI;

public class PauseMenu : MonoBehaviour
{
    public Button[] buttons;

    public void PauseMenuClose()
    {
        foreach (Button button in buttons)
        {
            button.onClick.RemoveAllListeners();
        }

        Destroy(gameObject);
    }
}

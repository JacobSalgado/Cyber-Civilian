using UnityEngine;
using UnityEngine.UI;

public class TransitionScreen : MonoBehaviour
{
    public Image image;
    [SerializeField] private Canvas canvas;

    void Start()
    {
        ChangeScreenAlpha(0f);
        canvas.sortingOrder = -1;
    }

    public void ChangeScreenAlpha(float newAlpha)
    {
        Color currentColor = image.color;
        currentColor.a = newAlpha;
        image.color = currentColor;
    }

    public void SetSortingOrder(int order)
    {
        canvas.sortingOrder = order;
    }
}

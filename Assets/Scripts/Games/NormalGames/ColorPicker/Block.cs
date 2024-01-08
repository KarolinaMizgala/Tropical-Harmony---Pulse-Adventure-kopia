using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Block : MonoBehaviour, IPointerDownHandler
{
    private Image image;
    private ColorPickerController gameController;

    public Color Color
    {
        set => image.color = value;
        get => image.color;
    }

    public void Setup(ColorPickerController gameController)
    {
        image = GetComponent<Image>();
        this.gameController = gameController;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        gameController.CheckBlock(Color);
    }
}


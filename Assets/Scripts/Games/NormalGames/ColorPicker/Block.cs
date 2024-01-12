using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

/// <summary>
/// Represents a block in the game.
/// </summary>
public class Block : MonoBehaviour, IPointerDownHandler
{
    private Image image;
    private ColorPickerController gameController;

    /// <summary>
    /// Gets or sets the color of the block.
    /// </summary>
    public Color Color
    {
        set => image.color = value;
        get => image.color;
    }

    /// <summary>
    /// Sets up the block with a reference to the game controller.
    /// </summary>
    /// <param name="gameController">The game controller.</param>
    public void Setup(ColorPickerController gameController)
    {
        image = GetComponent<Image>();
        this.gameController = gameController;
    }

    /// <summary>
    /// Called when the block is clicked.
    /// </summary>
    /// <param name="eventData">Data about the pointer event.</param>
    public void OnPointerDown(PointerEventData eventData)
    {
        gameController.CheckBlock(Color);
    }
}
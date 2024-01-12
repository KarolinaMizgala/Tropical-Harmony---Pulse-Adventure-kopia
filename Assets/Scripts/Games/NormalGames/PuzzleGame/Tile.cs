using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

/// <summary>
/// Represents a tile in the game.
/// </summary>
public class Tile : MonoBehaviour, IPointerClickHandler
{
    private TextMeshProUGUI textNumeric;
    private Board board;
    private Vector3 correctPosition;

    /// <summary>
    /// Gets a value indicating whether the tile is in the correct position.
    /// </summary>
    public bool IsCorrected { private set; get; } = false;

    private int numeric;

    /// <summary>
    /// Gets or sets the numeric value of the tile.
    /// </summary>
    public int Numeric
    {
        set
        {
            numeric = value;
            textNumeric.text = numeric.ToString();
        }
        get => numeric;
    }

    /// <summary>
    /// Sets up the tile.
    /// </summary>
    /// <param name="board">The game board.</param>
    /// <param name="hideNumeric">The numeric value to hide.</param>
    /// <param name="numeric">The numeric value of the tile.</param>
    public void Setup(Board board, int hideNumeric, int numeric)
    {
        this.board = board;
        textNumeric = GetComponentInChildren<TextMeshProUGUI>();

        Numeric = numeric;
        if (Numeric == hideNumeric)
        {
            GetComponent<UnityEngine.UI.Image>().enabled = false;
            textNumeric.enabled = false;
        }
    }

    /// <summary>
    /// Sets the correct position of the tile.
    /// </summary>
    public void SetCorrectPosition()
    {
        correctPosition = GetComponent<RectTransform>().localPosition;
    }

    /// <summary>
    /// Handles the PointerClick event of the tile.
    /// </summary>
    /// <param name="eventData">The event data.</param>
    public void OnPointerClick(PointerEventData eventData)
    {
        board.IsMoveTile(this);
    }

    /// <summary>
    /// Moves the tile to a specified position.
    /// </summary>
    /// <param name="end">The end position.</param>
    public void OnMoveTo(Vector3 end)
    {
        StartCoroutine("MoveTo", end);
    }

    /// <summary>
    /// Moves the tile to a specified position over time.
    /// </summary>
    /// <param name="end">The end position.</param>
    private IEnumerator MoveTo(Vector3 end)
    {
        float current = 0;
        float percent = 0;
        float moveTime = 0.1f;
        Vector3 start = GetComponent<RectTransform>().localPosition;

        while (percent < 1)
        {
            current += Time.deltaTime;
            percent = current / moveTime;

            GetComponent<RectTransform>().localPosition = Vector3.Lerp(start, end, percent);

            yield return null;
        }

        IsCorrected = correctPosition == GetComponent<RectTransform>().localPosition ? true : false;

        board.IsGameOver();
    }
}
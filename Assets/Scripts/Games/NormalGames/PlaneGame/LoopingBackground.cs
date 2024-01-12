using System.Collections;
using UnityEngine;

/// <summary>
/// Handles the looping of the background in the game.
/// </summary>
public class LoopingBackground : MonoBehaviour
{
    /// <summary>
    /// The speed at which the background moves.
    /// </summary>
    [SerializeField] private float backgroundSpeed;

    /// <summary>
    /// The renderer for the background.
    /// </summary>
    [SerializeField] Renderer backgroundRenderer;

    /// <summary>
    /// The tooltip GameObject.
    /// </summary>
    [SerializeField] private GameObject toolTip;

    /// <summary>
    /// Indicates whether the game has started.
    /// </summary>
    private bool gameStarted = false;

    /// <summary>
    /// Waits until the tooltip becomes inactive and then starts the game.
    /// </summary>
    private IEnumerator Start()
    {
        // Wait until the tooltip becomes inactive
        while (toolTip.activeSelf)
        {
            yield return null;
        }

        // Start the game
        gameStarted = true;
    }

    /// <summary>
    /// Updates the offset of the background texture every frame.
    /// </summary>
    void Update()
    {
        if (!gameStarted)
        {
            return;
        }
        backgroundRenderer.material.mainTextureOffset += new Vector2(backgroundSpeed * Time.deltaTime, 0);
    }
}
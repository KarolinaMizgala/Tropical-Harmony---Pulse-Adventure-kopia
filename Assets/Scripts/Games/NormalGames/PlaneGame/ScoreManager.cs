using System.Collections;
using TMPro;
using UnityEngine;

/// <summary>
/// Manages the score in the game.
/// </summary>
public class ScoreManager : MonoBehaviour
{
    /// <summary>
    /// The text component that displays the score.
    /// </summary>
    [SerializeField] private TMP_Text scoreText;

    /// <summary>
    /// The tooltip GameObject.
    /// </summary>
    [SerializeField] private GameObject toolTip;

    /// <summary>
    /// The current score.
    /// </summary>
    private float score;

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
    /// Updates the score every frame if the game has started and the player is not null.
    /// </summary>
    void Update()
    {
        if (!gameStarted)
        {
            return;
        }

        if (GameObject.FindGameObjectWithTag("Player") != null)
        {
            score += 1 * Time.deltaTime;
            scoreText.text = ((int)score).ToString();
        }
    }
}
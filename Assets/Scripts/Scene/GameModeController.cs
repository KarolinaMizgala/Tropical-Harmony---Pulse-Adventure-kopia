using UnityEngine;

/// <summary>
/// Enum representing the game modes.
/// </summary>
public enum GameMode
{
    ServerMode,
    ManualMode
}

/// <summary>
/// Class for controlling the game mode.
/// </summary>
public class GameModeController : MonoBehaviour
{
    /// <summary>
    /// The current game mode.
    /// </summary>
    private GameMode currentMode;

    /// <summary>
    /// Sets the game mode.
    /// </summary>
    /// <param name="mode">The game mode to set.</param>
    public void SetGameMode(GameMode mode)
    {
        currentMode = mode;
    }

    /// <summary>
    /// Gets the current game mode.
    /// </summary>
    /// <returns>The current game mode.</returns>
    public GameMode GetGameMode()
    {
        return currentMode; 
    }
}
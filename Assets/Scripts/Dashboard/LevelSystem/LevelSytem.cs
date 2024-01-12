using UnityEngine;

/// <summary>
/// Manages the level system in the game.
/// </summary>
public class LevelSystem : MonoBehaviour
{
    private const string LevelKey = "Level";
    private const string PointsKey = "Points";

    /// <summary>
    /// The current level.
    /// </summary>
    public int Level
    {
        get { return PlayerPrefs.GetInt(LevelKey, 1); }
        private set { PlayerPrefs.SetInt(LevelKey, value); }
    }

    /// <summary>
    /// The current points.
    /// </summary>
    public int Points
    {
        get { return PlayerPrefs.GetInt(PointsKey, 0); }
        private set { PlayerPrefs.SetInt(PointsKey, value); }
    }

    /// <summary>
    /// The name of the current level.
    /// </summary>
    public string LevelName
    {
        get { return GetLevelName(Level); }
    }

    /// <summary>
    /// Adds points and levels up if necessary.
    /// </summary>
    public void AddPoints(int points)
    {
        Points += points;
        PlayerPrefs.SetInt(PointsKey, Points);

        while (Points > Level * 50)
        {
            LevelUp();
        }
    }

    /// <summary>
    /// Increases the level by one.
    /// </summary>
    private void LevelUp()
    {
        Level++;
        PlayerPrefs.SetInt(LevelKey, Level);
    }

    /// <summary>
    /// Returns the name of the specified level.
    /// </summary>
    public string GetLevelName(int level)
    {
        switch (level)
        {
            case 1:
                return "Serenity Shores";
            case 2:
                return "Zen Oasis Levels";
            case 3:
                return "Calm Cove Stages";
            case 4:
                return "Tranquil Tropics Mastery";
            case 5:
                return "Palm Zenith Achievements";
            default:
                return "Unknown Level";
        }
    }
}
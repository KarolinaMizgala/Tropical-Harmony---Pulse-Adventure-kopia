using UnityEngine;

public class LevelSystem : MonoBehaviour
{
    private const string LevelKey = "Level";
    private const string PointsKey = "Points";

    public int Level
    {
        get { return PlayerPrefs.GetInt(LevelKey, 1); }
        private set { PlayerPrefs.SetInt(LevelKey, value); }
    }

    public int Points
    {
        get { return PlayerPrefs.GetInt(PointsKey, 0); }
        private set { PlayerPrefs.SetInt(PointsKey, value); }
    }

    public string LevelName
    {
        get { return GetLevelName(Level); }
    }

    public void AddPoints(int points)
    {
        Points += points;
        PlayerPrefs.SetInt(PointsKey, Points);

        while (Points > Level * 50)
        {
            LevelUp();
        }
    }

    private void LevelUp()
    {
        Level++;
        PlayerPrefs.SetInt(LevelKey, Level);
    }

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
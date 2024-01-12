using UnityEngine;

/// <summary>
/// Manages the player's position in the game.
/// </summary>
public class PlayerPositionManager : MonoBehaviour
{
    /// <summary>
    /// Saves the player's position.
    /// </summary>
    /// <param name="position">The position to save.</param>
    public void SavePlayerPosition(Vector3 position)
    {
        PlayerPrefs.SetFloat("PlayerPositionX", position.x);
        PlayerPrefs.SetFloat("PlayerPositionY", position.y);
        PlayerPrefs.SetFloat("PlayerPositionZ", position.z);
        PlayerPrefs.Save();
    }

    /// <summary>
    /// Loads the player's position.
    /// </summary>
    /// <returns>The loaded position.</returns>
    public Vector3 LoadPlayerPosition()
    {
        if (PlayerPrefs.HasKey("PlayerPositionX") && PlayerPrefs.HasKey("PlayerPositionY") && PlayerPrefs.HasKey("PlayerPositionZ"))
        {
            float x = PlayerPrefs.GetFloat("PlayerPositionX");
            float y = PlayerPrefs.GetFloat("PlayerPositionY");
            float z = PlayerPrefs.GetFloat("PlayerPositionZ");

            return new Vector3(x, y, z);
        }

        // Default position if there is no saved player position
        return Vector3.zero;
    }
}
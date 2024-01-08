using UnityEngine;

public class PlayerPositionManager : MonoBehaviour
{
    // Metoda do zapisywania pozycji gracza
    public void SavePlayerPosition(Vector3 position)
    {
        PlayerPrefs.SetFloat("PlayerPositionX", position.x);
        PlayerPrefs.SetFloat("PlayerPositionY", position.y);
        PlayerPrefs.SetFloat("PlayerPositionZ", position.z);
        PlayerPrefs.Save();
    }

    // Metoda do wczytywania pozycji gracza
    public Vector3 LoadPlayerPosition()
    {
        if (PlayerPrefs.HasKey("PlayerPositionX") && PlayerPrefs.HasKey("PlayerPositionY") && PlayerPrefs.HasKey("PlayerPositionZ"))
        {
            float x = PlayerPrefs.GetFloat("PlayerPositionX");
            float y = PlayerPrefs.GetFloat("PlayerPositionY");
            float z = PlayerPrefs.GetFloat("PlayerPositionZ");

            return new Vector3(x, y, z);
        }

        // Domyœlna pozycja, jeœli nie ma zapisanej pozycji gracza
        return Vector3.zero;
    }
}

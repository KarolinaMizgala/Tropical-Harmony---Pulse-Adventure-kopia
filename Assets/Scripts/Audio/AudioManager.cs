using UnityEngine;

public class AudioManager : MonoBehaviour
{
    private AudioSource audioSource;

    // Parametr mutacji - pitch
    public static bool isMute = false;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    void Update()
    {
        // SprawdŸ, czy dŸwiêk jest odtwarzany
        if (audioSource.isPlaying)
        {
            // Zastosuj mutacjê pitch'a
            audioSource.mute = isMute;
        }
    }
}

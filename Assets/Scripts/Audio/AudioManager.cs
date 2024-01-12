using UnityEngine;

/// <summary>
/// Manages the audio in the game.
/// </summary>
public class AudioManager : MonoBehaviour
{
    /// <summary>
    /// The AudioSource component.
    /// </summary>
    private AudioSource audioSource;

    /// <summary>
    /// Indicates whether the audio is muted.
    /// </summary>
    public static bool isMute = false;

    /// <summary>
    /// Initializes the AudioSource component.
    /// </summary>
    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    /// <summary>
    /// Updates the mute state of the AudioSource component every frame.
    /// </summary>
    void Update()
    {
        // Check if the audio is playing
        if (audioSource.isPlaying)
        {
            // Apply the mute state
            audioSource.mute = isMute;
        }
    }
}
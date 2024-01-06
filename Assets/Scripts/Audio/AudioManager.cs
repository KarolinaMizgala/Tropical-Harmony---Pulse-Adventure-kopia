using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

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
        // Sprawd�, czy d�wi�k jest odtwarzany
        if (audioSource.isPlaying)
        {
            // Zastosuj mutacj� pitch'a
            audioSource.mute = isMute;
        }
    }
}

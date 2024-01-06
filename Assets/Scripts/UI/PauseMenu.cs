using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

/// <summary>
/// Class responsible for handling the pause menu UI and its interactions.
/// </summary>
public class PauseMenu : MonoBehaviour
{
    /// <summary>
    /// SceneService instance for scene transitions.
    /// </summary>
    [Inject] 
    private SceneService sceneService;

    /// <summary>
    /// GameObject for the mute button.
    /// </summary>
    [SerializeField] 
    private GameObject muteButton;

    /// <summary>
    /// GameObject for the unmute button.
    /// </summary>
    [SerializeField] 
    private GameObject unmuteButton;

    /// <summary>
    /// GameObject for the pause UI.
    /// </summary>
    [SerializeField] 
    private GameObject pauseUI;

    /// <summary>
    /// Handles the click event on the Resume button.
    /// </summary>
    public void onResumeClick()
    {
        pauseUI.SetActive(false);
    }

    /// <summary>
    /// Handles the click event on the Mute button.
    /// </summary>
    public void onMuteClick()
    {
        if (muteButton.activeSelf)
        {
            muteButton.SetActive(false);
            unmuteButton.SetActive(true);
            AudioManager.isMute = true;
        }
        else
        {
            muteButton.SetActive(true);
            unmuteButton.SetActive(false);
            AudioManager.isMute = false;
        }
    }

    /// <summary>
    /// Handles the click event on the Menu button.
    /// </summary>
    public void onMenuClick()
    {
        sceneService.LoadScene(SceneType.MainMenu);
    }

    /// <summary>
    /// Handles the click event on the Exit button.
    /// </summary>
    public void onExitClick()
    {
        Application.Quit();
    }
}
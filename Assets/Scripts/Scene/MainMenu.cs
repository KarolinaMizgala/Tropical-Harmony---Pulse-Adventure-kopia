using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Zenject;

/// <summary>
/// Class representing the main menu.
/// </summary>
public class MainMenu : MonoBehaviour
{
    /// <summary>
    /// Input field for user input.
    /// </summary>
    [SerializeField] TMP_InputField inputField;

    /// <summary>
    /// Play button GameObject.
    /// </summary>
    [SerializeField] private GameObject playButton;

    /// <summary>
    /// Controller button container GameObject.
    /// </summary>
    [SerializeField] private GameObject controllerButtonContainer;

    /// <summary>
    /// Manual button container GameObject.
    /// </summary>
    [SerializeField] private GameObject manualButtonContainer;

    /// <summary>
    /// WebSocket client for network communication.
    /// </summary>
    [Inject] private WebSocketClient webSocketClient;

    /// <summary>
    /// Scene service for scene management.
    /// </summary>
    [Inject] private SceneService sceneService;

    /// <summary>
    /// Dialog system for displaying dialogs.
    /// </summary>
    [Inject] private DialogSystem dialogSystem;

    /// <summary>
    /// Game mode controller for controlling the game mode.
    /// </summary>
    [Inject] private GameModeController gameModeController;

    /// <summary>
    /// Heart rate of the user.
    /// </summary>
    private float heartRate;

    /// <summary>
    /// Threshold for rest heart rate.
    /// </summary>
    private float restThreshold = 100f;

    /// <summary>
    /// Threshold for time.
    /// </summary>
    private float timeThreshold = 120f;

    /// <summary>
    /// Elapsed time since the last scene change.
    /// </summary>
    private float elapsedTime = 0f;

    /// <summary>
    /// Updates the scene based on the heart rate and elapsed time.
    /// </summary>
    void Update()
    {

        if (webSocketClient.IsConnected())
        {
            // Updating time
            elapsedTime += Time.deltaTime;

            // Check scene change conditions
            if (heartRate > restThreshold)
            {
                if (elapsedTime >= timeThreshold)
                {
                    sceneService.LoadScene(SceneType.RestfulScene);
                    elapsedTime = 0f;
                }
            }
            else if (heartRate <= restThreshold)
            {
                // Reset time when heart rate is below threshold
                if (elapsedTime >= timeThreshold)
                {
                    sceneService.LoadScene(SceneType.Energetic);
                    elapsedTime = 0f;
                }
            }
        }
        if (Application.internetReachability != NetworkReachability.NotReachable)
        {
            playButton.SetActive(true);
           
        }
    }

    /// <summary>
    /// Checks the internet connectivity at the start of the game.
    /// </summary>
    private void Start()
    {
        if (Application.internetReachability == NetworkReachability.NotReachable)
        {
            playButton.SetActive(false);
            dialogSystem.ShowConfirmationDialog("You are not connected to the internet. Do you want to start the game manually?", OnManualClick, null);
        }
    }

    /// <summary>
    /// Handles the play button click event.
    /// </summary>
    public async void OnPlayClick()
    {
        if (inputField != null)
        {
            gameModeController.SetGameMode(GameMode.ServerMode);
            await webSocketClient.ConnectAsync(inputField.text);

        }
    }

    /// <summary>
    /// Handles the manual button click event.
    /// </summary>
    public void OnManualClick()
    {
        inputField.gameObject.SetActive(false);
        controllerButtonContainer.SetActive(false);
        manualButtonContainer.SetActive(true);
        gameModeController.SetGameMode(GameMode.ManualMode);
    }

    /// <summary>
    /// Handles the restful button click event.
    /// </summary>
    public void OnRestfulClick()
    {
        sceneService.LoadScene(SceneType.RestfulScene);
    }

    /// <summary>
    /// Handles the energetic button click event.
    /// </summary>
    public void OnEnergeticClick()
    {
        sceneService.LoadScene(SceneType.Energetic);
    }

    /// <summary>
    /// Handles the exit button click event.
    /// </summary>
    public void OnExit()
    {
        Application.Quit();
    }
}
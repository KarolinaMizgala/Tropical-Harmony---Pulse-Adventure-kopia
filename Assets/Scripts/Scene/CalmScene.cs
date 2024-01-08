using TMPro;
using UnityEngine;
using Zenject;
/// <summary>
/// Class representing a restful scene in the game.
/// </summary>
public class CalmScene : MonoBehaviour
{
    [Inject] WebSocketClient webSocketClient; ///< The WebSocket client.
    [Inject] SceneService sceneService; ///< The scene service.
    [Inject] DialogSystem dialogSystem; ///< The dialog system.
    [Inject] GameModeController gameModeController; ///< The game mode controller.
    [Inject] PlayerPositionManager playerPositionManager; ///< The player position manager.

    [SerializeField] private GameObject pauseMenu; ///< The pause menu.
    [SerializeField] private GameObject gameUI; ///< The game UI.
    [SerializeField] private TMP_Text bmp; ///< The text component for displaying the heart rate.

    [SerializeField] private GameObject ManualModeUI; ///< The UI for manual mode.
    [SerializeField] private GameObject ServerModeUI; ///< The UI for server mode.

    [SerializeField] private GameObject player; ///< The player.

    private bool pauseMenuActive = false; ///< Whether the pause menu is active.

    private float restThreshold = 100f; ///< The rest threshold.
    private float timeThreshold = 120f; ///< The time threshold for changing the scene.
    private float elapsedTimeAboveThreshold = 0f; ///< The elapsed time above the threshold.

    private bool isChanged = false; ///< Whether the scene has been changed.
    private bool isDialogShowing = false; ///< Whether the dialog is showing.

    private bool shouldChangeScene = true; ///< Whether the scene should be changed.

    private bool isModeChanged = false; ///< Whether the mode has been changed.
    private GameMode gameMode; ///< The current game mode.
    public static bool isSceneOpened; ///< Whether the scene has been opened.


    /// <summary>
    /// Awake is called when the script instance is being loaded.
    /// </summary>
    private void Awake()
    {
        if (gameModeController.GetGameMode() == GameMode.ServerMode)
        {
            ManualModeUI.SetActive(false);
            ServerModeUI.SetActive(true);
            gameMode = GameMode.ServerMode;

        }
        else
        {
            ManualModeUI.SetActive(true);
            ServerModeUI.SetActive(false);
            gameMode = GameMode.ManualMode;
        }

        player = GameObject.FindGameObjectWithTag("Player");


    }
    /// <summary>
    /// Update is called every frame, if the MonoBehaviour is enabled.
    /// </summary>
    void Update()
    {
        if (webSocketClient != null && webSocketClient.IsConnected())
        {
            float receivedData = webSocketClient.GetReceivedData();
            if (!string.IsNullOrEmpty(receivedData.ToString()))
            {
                // Update the TextMeshPro component with the received data
                bmp.text = receivedData.ToString();

                // Check if heart rate is above 110
                if (receivedData < restThreshold)
                {
                    elapsedTimeAboveThreshold += Time.deltaTime;

                    if (elapsedTimeAboveThreshold > timeThreshold && shouldChangeScene)
                    {
                        // Change scene when heart rate is above 110 for the specified time
                        ShowChangeSceneDialog();
                        elapsedTimeAboveThreshold = 0f;
                    }
                }
                else
                {
                    // Reset the timer when heart rate goes below 110
                    elapsedTimeAboveThreshold = 0f;
                }
            }
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            pauseMenu.SetActive(!pauseMenuActive);
            gameUI.SetActive(pauseMenuActive);
            pauseMenuActive = !pauseMenuActive;
        }
        if (gameModeController.GetGameMode() == GameMode.ManualMode && gameMode != GameMode.ManualMode)
        {
            ManualModeUI.SetActive(true);
            ServerModeUI.SetActive(false);
        }
    }
    /// <summary>
    /// Update is called every frame, if the MonoBehaviour is enabled.
    /// </summary>
    private void OnEnable()
    {
        if (player != null)
        {
            if (PlayerPrefs.HasKey("PlayerPositionX") && PlayerPrefs.HasKey("PlayerPositionY") && PlayerPrefs.HasKey("PlayerPositionZ"))
            {
                var newPosition = playerPositionManager.LoadPlayerPosition();
                player.transform.position = newPosition;
            }
            else
            {
                player.transform.position = new Vector3(24.7800007f, 4, 81);
            }
        }
        isDialogShowing = true;
    }
    /// <summary>
    /// This function is called when the behaviour becomes disabled or inactive.
    /// </summary>
    private void OnDisable()
    {
        if (player != null)
        {
            playerPositionManager.SavePlayerPosition(player.transform.position);
        }
    }
    /// <summary>
    /// Show a dialog for changing the scene.
    /// </summary>
    private void ShowChangeSceneDialog()
    {
        if (isDialogShowing) return;
        isDialogShowing = true;
        dialogSystem.ShowConfirmationDialog("The heart rate is sufficiently low. Consider transitioning to an energetic scene for an optimal experience. Would you like to proceed to an energetic scene?", ChangeScene, ResetIsDialogShowing);

    }
    /// <summary>
    /// Reset the dialog showing flag.
    /// </summary>
    private void ResetIsDialogShowing()
    {
        shouldChangeScene = false;
        isDialogShowing = false;
    }
    /// <summary>
    /// Change the scene.
    /// </summary>
    private void ChangeScene()
    {
        ResetIsDialogShowing();
        if (isChanged) return;
        isChanged = true;
        sceneService.LoadScene(SceneType.Energetic);
        isChanged = false;
    }
    /// <summary>
    /// Handle the button click event.
    /// </summary>
    public void onButtonClick()
    {
        sceneService.LoadScene(SceneType.Energetic);
    }
}

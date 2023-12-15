using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Zenject;

public class CalmScene : MonoBehaviour
{
    [Inject] WebSocketClient webSocketClient;
    [Inject] SceneService sceneService;
    [Inject] DialogSystem dialogSystem;
    [Inject] GameModeController gameModeController;
    [Inject] PlayerPositionManager playerPositionManager;

    [SerializeField] private GameObject pauseMenu;
    [SerializeField] private GameObject gameUI;
    [SerializeField] private TMP_Text bmp;

    [SerializeField] private GameObject ManualModeUI;
    [SerializeField] private GameObject ServerModeUI;

    [SerializeField] private GameObject player;

    private bool pauseMenuActive = false;

    private float restThreshold = 100f;
    private float timeThreshold = 120f;  // Czas (w sekundach) powy¿ej 110, aby zmieniæ scenê
    private float elapsedTimeAboveThreshold = 0f;

    private bool isChanged = false;
    private bool isDialogShowing = false;

   private bool shouldChangeScene = true;

  

    private void Awake()
    {
        if(gameModeController.GetGameMode()==GameMode.ServerMode)
        {
            ManualModeUI.SetActive(false);
            ServerModeUI.SetActive(true);
        }
        else
        {
            ManualModeUI.SetActive(true);
            ServerModeUI.SetActive(false);
        }

        player = GameObject.FindGameObjectWithTag("Player");
    }
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
    }
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
    }
    private void OnDisable()
    {
        if (player != null)
        {
            playerPositionManager.SavePlayerPosition(player.transform.position);
        }
    }
    private void ShowChangeSceneDialog()
    {
        if (isDialogShowing) return;
        isDialogShowing = true;
        dialogSystem.ShowConfirmationDialog("The heart rate is sufficiently low. Consider transitioning to an energetic scene for an optimal experience. Would you like to proceed to an energetic scene?", ChangeScene, ResetIsDialogShowing);
      
    }
     private void ResetIsDialogShowing()
    {
        shouldChangeScene = false;
        isDialogShowing=false;
    }
    private void ChangeScene()
    {
        ResetIsDialogShowing();
        if (isChanged) return;
        isChanged = true; 
        sceneService.LoadScene(SceneType.Energetic);
        isChanged = false;
    }
    public void onButtonClick()
    {
        sceneService.LoadScene(SceneType.Energetic);
    }
}

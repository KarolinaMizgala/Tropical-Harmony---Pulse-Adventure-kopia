using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Zenject;

public class EnergeticScene : MonoBehaviour
{
    [Inject] WebSocketClient webSocketClient;
    [Inject] SceneService sceneService;

    [SerializeField] private GameObject pauseMenu;
    [SerializeField] private GameObject gameUI;
    [SerializeField] private TMP_Text bmp;

    private bool pauseMenuActive = false; 
    void Update()
    {
       
        if (webSocketClient != null && webSocketClient.IsConnected())
            {
                float receivedData = webSocketClient.GetReceivedData(); 
                if (!string.IsNullOrEmpty(receivedData.ToString()))
                {
                    // Update the TextMeshPro component with the received data
                    bmp.text = receivedData.ToString();
                }
            }
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            pauseMenu.SetActive(!pauseMenuActive);
            gameUI.SetActive(pauseMenuActive);
            pauseMenuActive = !pauseMenuActive;
        }
    }
    public void onButtonClick()
    {
        sceneService.LoadScene(SceneType.RestfulScene);
    }    

}

using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Zenject;

public class MainMenu : MonoBehaviour
{
    [SerializeField] TMP_InputField inputField;
    [SerializeField] private GameObject playButton;

    [SerializeField] private GameObject controllerButtonContainer;
    [SerializeField] private GameObject manualButtonContainer;

    [Inject] private WebSocketClient webSocketClient;
    [Inject] private SceneService sceneService;
    [Inject] private SceneChanger sceneChanger;

    private float heartRate;
    private float restThreshold = 100f;
    private float timeThreshold = 120f;
    private float elapsedTime = 0f;

    void Update()
    {
        // Pobieranie têtna (przyjmujemy, ¿e jest to dostêpne w zmiennej heartRate)
        // Replace this with your actual heart rate retrieval logic.
        if (webSocketClient.IsConnected())
        {
            // Aktualizacja czasu
            elapsedTime += Time.deltaTime;

            // SprawdŸ warunki zmiany sceny
            if (heartRate > restThreshold)
            {
                if (elapsedTime >= timeThreshold)
                {
                    sceneChanger.ChangeRestfulScene();
                    elapsedTime = 0f;
                }
            }
            else if (heartRate <= restThreshold)
            {
                // Resetuj czas, gdy têtno jest poni¿ej progu
                if (elapsedTime >= timeThreshold)
                {
                    sceneChanger.ChangeEnergeticScene();
                    elapsedTime = 0f;
                }
            }
        }
        if (Application.internetReachability == NetworkReachability.NotReachable)
        {
            playButton.SetActive(false);
        }
    }

    public async void onPlayClick()
    {
        if (inputField != null)
        {

            await webSocketClient.ConnectAsync(inputField.text);

        }
    }

    public void onManualClick()
    {
        inputField.gameObject.SetActive(false);
        controllerButtonContainer.SetActive(false);
        manualButtonContainer.SetActive(true);
    }

    public void onRestfulClick()
    {
        sceneChanger.ChangeRestfulScene();
    }
    public void onEnergeticClick()
    {
        sceneChanger.ChangeEnergeticScene();
    }

}

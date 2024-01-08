using Azure.Messaging.WebPubSub;
using NativeWebSocket;
using System;
using System.Globalization;
using System.Threading.Tasks;
using UnityEngine;
using Zenject;

/// <summary>
/// Class responsible for handling WebSocket connections.
/// </summary>
public class WebSocketClient : MonoBehaviour
{
    /// WebSocket connection.
    WebSocket websocket;
    /// Data received from the WebSocket connection.
    private float receivedData;

    /// Unique ID for the WebSocket connection.
    private string clientId;

    /// Time when the last message was received.
    private float lastMessageReceivedTime;

    /// Flag indicating if a dialog is currently shown.
    private bool isDialogShown = false;

    [Inject] private SceneService sceneService;
    [Inject] private DialogSystem dialogSystem;
    [Inject] private GameModeController gameMode;
    [Inject] private PulseStatsRecorder pulseStatsRecorder;

    private float timer = 5f;
    private float recordingInterval = 0f;


    /// <summary>
    /// Connects to the WebSocket server with a unique ID.
    /// </summary>
    /// <param name="uniqueId">Unique ID for the WebSocket connection.</param>
    public async void Connect(string uniqueId)
    {
        clientId = uniqueId;
        await ConnectAsync(uniqueId);
    }

    /// <summary>
    /// Reconnects to the WebSocket server with the previously used unique ID.
    /// </summary>
    public async void Reconnect()
    {
        await ConnectAsync(clientId);
    }

    /// <summary>
    /// Asynchronously connects to the WebSocket server with a unique ID.
    /// </summary>
    /// <param name="uniqueId">Unique ID for the WebSocket connection.</param>
    public async Task ConnectAsync(string uniqueId)
    {
        var connectionString = "Endpoint=https://chatapp-wps-lici52g2mb2vo.webpubsub.azure.com;AccessKey=ZZOZ5UAzuS9mhrrcOL+jG7RHQ8zgCuCeYTtQUQg3yyI=;Version=1.0;";
        var hub = "sample_chat";

        var serviceClient = new WebPubSubServiceClient(connectionString, hub);
        var url = serviceClient.GetClientAccessUri(userId: $"Unity{uniqueId}", roles: new string[] { "webpubsub.joinLeaveGroup", "webpubsub.sendToGroup" });

        websocket = new WebSocket(url.ToString());


        websocket.OnOpen += () =>
        {
            serviceClient.AddUserToGroup(uniqueId, $"Unity{uniqueId}");
            Debug.Log("Connection open!");
            if (receivedData > 100)
            {
                sceneService.LoadScene(SceneType.RestfulScene);

            }
            else
            {
                sceneService.LoadScene(SceneType.Energetic);
            }

        };

        websocket.OnError += (e) =>
        {
            Debug.Log("Error! " + e);
            isDialogShown = true;
            dialogSystem.ShowConfirmationDialog("Connection failed. Do you want to try connecting again?", Reconnect, ChangeGameMode);
        };

        websocket.OnClose += (e) =>
        {
            Debug.Log("Connection closed!");
            if (isDialogShown) return;
            dialogSystem.ShowConfirmationDialog("Connection closed. Do you want to try connecting again?", Reconnect, ChangeGameMode);
            isDialogShown = false;
        };

        websocket.OnMessage += (bytes) =>
        {
            var message = System.Text.Encoding.UTF8.GetString(bytes);
            Debug.Log("Received OnMessage! (" + bytes.Length + " bytes) " + message);
            lastMessageReceivedTime = Time.time;
            try
            {

                float heartRate = float.Parse(message, CultureInfo.InvariantCulture.NumberFormat);
                if (heartRate != 0)
                {
                    if (timer <= recordingInterval)
                    {
                        pulseStatsRecorder.SavePulseData(heartRate);
                        timer = 5f;
                    }
                    receivedData = heartRate;

                }
            }
            catch (FormatException ex)
            {
                Debug.Log("Error parsing heart rate: " + ex.Message);
            }

        };

        await websocket.Connect();
    }

    /// <summary>
    /// Unity's Update method, called once per frame.
    /// </summary>
    void Update()
    {
        timer -= Time.deltaTime;
#if !UNITY_WEBGL || UNITY_EDITOR
        if (IsConnected())
            websocket.DispatchMessageQueue();

        if (Time.time - lastMessageReceivedTime >= 60f && IsConnected())
        {
            dialogSystem.ShowConfirmationDialog("No messages have been received for 1 minute. Please check your watch. Do you want to try connecting again?", Reconnect, ChangeGameMode);
            lastMessageReceivedTime = Time.time; // resetujemy timer, aby dialog nie pojawiał się co klatkę
        }

#endif
    }


    /// <summary>
    /// Checks if the WebSocket connection is open.
    /// </summary>
    /// <returns>True if the WebSocket connection is open, false otherwise.</returns>
    public bool IsConnected()
    {
        return websocket?.State == WebSocketState.Open;
    }

    /// <summary>
    /// Gets the data received from the WebSocket connection.
    /// </summary>
    /// <returns>The data received from the WebSocket connection.</returns>
    public float GetReceivedData()
    {
        return receivedData;
    }

    /// <summary>
    /// Changes the game mode to ManualMode.
    /// </summary>
    private void ChangeGameMode()
    {
        gameMode.SetGameMode(GameMode.ManualMode);
    }
}
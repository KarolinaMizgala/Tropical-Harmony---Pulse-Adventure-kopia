using System;
using UnityEngine;
using UnityEngine.UI;
using System.Text;
using System.Threading;
using System.Collections;
using System.Threading.Tasks;
using TMPro;

using NativeWebSocket;
using Zenject;
using System.Globalization;
using UnityEngine.Events;
using System.Xml;
using System.Collections.Generic;
using System.IO;

public class WebSocketClient : MonoBehaviour
{
    WebSocket websocket;
    private float receivedData;

    [Inject] private SceneService sceneService;
    [Inject] private SceneChanger changer;
    [Inject] private DialogSystem dialogSystem;
    [Inject] private GameModeController gameMode;
    [Inject] private PulseStatsRecorder pulseStatsRecorder;

    private string clientId;
    private float lastMessageReceivedTime;
    private bool isDialogShown = false;

    public async void Connect(string uniqueId)
    {
        // Sprawdzamy, czy wprowadzone ID r�ni si� od bie��cego ID
        clientId = uniqueId;
        await ConnectAsync(uniqueId);

    }
    public async void Reconnect()
    {
        await ConnectAsync(clientId);
    }
    public async Task ConnectAsync(string uniqueId)
    {
        websocket = new WebSocket($"wss://free.blr2.piesocket.com/v3/{uniqueId}?api_key=OXAlmQ71QwVgQEwYxTzmqnGUrw6Cg0TfOmkAVmoU&notify_self=1");

        websocket.OnOpen += () =>
        {
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
            dialogSystem.ShowConfirmationDialog("Connection failed. Do you want to try connecting again?", Reconnect, BackToChangeGameMode);
        };

        websocket.OnClose += (e) =>
        {
            Debug.Log("Connection closed!");
            if (isDialogShown) return; 
            dialogSystem.ShowConfirmationDialog("Connection closed. Do you want to try connecting again?", Reconnect, BackToChangeGameMode);
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
                    receivedData = heartRate;
                    pulseStatsRecorder.SavePulseDataToFile(heartRate);
                }
            }
            catch (FormatException ex)
            {
                Debug.Log("Error parsing heart rate: " + ex.Message);
            }

        };

        await websocket.Connect();
    }

    void Update()
    {
#if !UNITY_WEBGL || UNITY_EDITOR
        if (IsConnected())
            websocket.DispatchMessageQueue();

            if (Time.time - lastMessageReceivedTime >= 60f && IsConnected())
    {
        dialogSystem.ShowConfirmationDialog("No messages have been received for 1 minute. Please check your watch. Do you want to try connecting again?", Reconnect, BackToChangeGameMode);
        lastMessageReceivedTime = Time.time; // resetujemy timer, aby dialog nie pojawiał się co klatkę
    }
#endif
    }

    public bool IsConnected()
    {
        return websocket?.State == WebSocketState.Open;
    }

    public float GetReceivedData()
    {
        return receivedData;
    }
    private void BackToChangeGameMode()
    {
        gameMode.SetGameMode(GameMode.ManualMode);
    }
   
    }

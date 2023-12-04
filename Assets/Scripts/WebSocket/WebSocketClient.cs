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

public class WebSocketClient : MonoBehaviour
{
    WebSocket websocket;
    private float receivedData;

    [Inject] private SceneService sceneService;
    [Inject] private SceneChanger changer;

    public async void Connect(string uniqueId)
    {
        // Sprawdzamy, czy wprowadzone ID ró¿ni siê od bie¿¹cego ID
      
            await ConnectAsync(uniqueId);
   
    }

    public async Task ConnectAsync(string uniqueId)
    {
        websocket = new WebSocket($"ws://localhost:8080/ws/{uniqueId}");

        websocket.OnOpen += () =>
        {
            Debug.Log("Connection open!");
            if (receivedData > 100)
            {
                changer.ChangeRestfulScene();

                }
            else
            {
                changer.ChangeRestfulScene();
            }
         
        };

        websocket.OnError += (e) =>
        {
            Debug.Log("Error! " + e);
        };

        websocket.OnClose += (e) =>
        {
            Debug.Log("Connection closed!");
           
        };

        websocket.OnMessage += (bytes) =>
        {
            var message = System.Text.Encoding.UTF8.GetString(bytes);
            Debug.Log("Received OnMessage! (" + bytes.Length + " bytes) " + message);

            try
            {
                
                float heartRate = float.Parse(message, CultureInfo.InvariantCulture.NumberFormat);

                
                receivedData = heartRate;
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
    

}

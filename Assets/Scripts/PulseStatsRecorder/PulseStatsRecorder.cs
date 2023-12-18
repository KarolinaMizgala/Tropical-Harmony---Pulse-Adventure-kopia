using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using Newtonsoft.Json;

[Serializable]
public class PulseData
{
    public string timestamp;
    public float pulseValue;
}

public class PulseStatsRecorder : MonoBehaviour
{
    private float recordingInterval = 5f; // Zapisuj co 5 sekund
    private float timer = 0f;

    private List<PulseData> pulseDataList = new List<PulseData>();

    public void SavePulseDataToFile(float heartRate)
    {
        PulseData pulseData = new PulseData
        {
            timestamp = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"),
            pulseValue = heartRate
        };

        string folderPath = Application.persistentDataPath;
        string filePath = Path.Combine(folderPath, "PulseData.json");

        if (!Directory.Exists(folderPath))
        {
            Directory.CreateDirectory(folderPath);
        }

        List<PulseData> pulseDataList = new List<PulseData>();

        if (File.Exists(filePath))
        {
            string existingData = File.ReadAllText(filePath);

            // SprawdŸ, czy plik zawiera obiekt czy tablicê
            if (!string.IsNullOrEmpty(existingData) && existingData.TrimStart().StartsWith("{"))
            {
                // Jeœli to obiekt, zamieñ go na tablicê jednoelementow¹
                pulseDataList.Add(JsonConvert.DeserializeObject<PulseData>(existingData));
            }
            else
            {
                // Jeœli to ju¿ tablica, zdeserializuj normalnie
                pulseDataList = JsonConvert.DeserializeObject<List<PulseData>>(existingData);
            }
        }

        pulseDataList.Add(pulseData);
        string jsonData = JsonConvert.SerializeObject(pulseDataList, Formatting.Indented);
        File.WriteAllText(filePath, jsonData);

        Debug.Log("Pulse data saved to: " + filePath);
        Debug.Log("JSON data: " + jsonData);
    }

}

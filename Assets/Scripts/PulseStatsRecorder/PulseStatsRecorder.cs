using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Records pulse data in the game.
/// </summary>
public class PulseStatsRecorder : MonoBehaviour
{
    private List<float> pulseDataList = new List<float>(); // List to store pulse data

    /// <summary>
    /// Saves pulse data.
    /// </summary>
    /// <param name="pulseData">The pulse data to save.</param>
    public void SavePulseData(float pulseData)
    {
        pulseDataList.Add(pulseData);
    }

    /// <summary>
    /// Gets the list of pulse data.
    /// </summary>
    /// <returns>The list of pulse data.</returns>
    public List<float> GetPulseDataList()
    {
        return pulseDataList;
    }

    /// <summary>
    /// Clears the list of pulse data.
    /// </summary>
    public void ClearPulseDataList()
    {
        pulseDataList.Clear();
    }
}
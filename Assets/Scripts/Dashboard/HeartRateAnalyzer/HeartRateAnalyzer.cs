using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Zenject;

/// <summary>
/// Manages the heart rate statistics in the game.
/// </summary>
public class HeartRateStatistics : MonoBehaviour
{
    /// <summary>
    /// The list of heart rates.
    /// </summary>
    private List<float> heartRatesList = new List<float>();

    /// <summary>
    /// The pulse stats recorder.
    /// </summary>
    [Inject] private PulseStatsRecorder pulseStatsRecorder;

    /// <summary>
    /// The text component that displays the minimum heart rate.
    /// </summary>
    [SerializeField] private TMPro.TMP_Text minimumHeartRate;

    /// <summary>
    /// The text component that displays the maximum heart rate.
    /// </summary>
    [SerializeField] private TMPro.TMP_Text maximumHeartRate;

    /// <summary>
    /// The text component that displays the average heart rate.
    /// </summary>
    [SerializeField] private TMPro.TMP_Text averageHeartRate;

    /// <summary>
    /// Initializes the heart rate statistics.
    /// </summary>
    private void Start()
    {
        heartRatesList = pulseStatsRecorder.GetPulseDataList();
        minimumHeartRate.text = GetMinimumHeartRate().ToString();
        maximumHeartRate.text = GetMaximumHeartRate().ToString();
        averageHeartRate.text = GetAverageHeartRate().ToString();
    }

    /// <summary>
    /// Returns the minimum heart rate.
    /// </summary>
    public float GetMinimumHeartRate()
    {
        if (heartRatesList.Count == 0)
        {
            return 0; // or another default value
        }

        return heartRatesList.Min();
    }

    /// <summary>
    /// Returns the maximum heart rate.
    /// </summary>
    public float GetMaximumHeartRate()
    {
        if (heartRatesList.Count == 0)
        {
            return 0; // or another default value
        }

        return heartRatesList.Max();
    }

    /// <summary>
    /// Returns the average heart rate.
    /// </summary>
    public int GetAverageHeartRate()
    {
        if (heartRatesList.Count == 0)
        {
            return 0; // or another default value
        }

        return (int)heartRatesList.Average();
    }

    /// <summary>
    /// Clears the heart rate statistics.
    /// </summary>
    public void ClearStatistics()
    {
        heartRatesList.Clear();
    }
}
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Zenject;

public class HeartRateStatistics : MonoBehaviour
{
    private List<float> heartRatesList = new List<float>();
    [Inject] private PulseStatsRecorder pulseStatsRecorder;

    [SerializeField] private TMPro.TMP_Text minimumHeartRate;
    [SerializeField] private TMPro.TMP_Text maximumHeartRate;
    [SerializeField] private TMPro.TMP_Text averageHeartRate;

    private void Start()
    {
        heartRatesList = pulseStatsRecorder.GetPulseDataList();
        minimumHeartRate.text = GetMinimumHeartRate().ToString();
        maximumHeartRate.text = GetMaximumHeartRate().ToString();
        averageHeartRate.text = GetAverageHeartRate().ToString();
    }


    public float GetMinimumHeartRate()
    {
        if (heartRatesList.Count == 0)
        {
            return 0; // lub inna wartoœæ domyœlna
        }

        return heartRatesList.Min();
    }

    public float GetMaximumHeartRate()
    {
        if (heartRatesList.Count == 0)
        {
            return 0; // lub inna wartoœæ domyœlna
        }

        return heartRatesList.Max();
    }

    public int GetAverageHeartRate()
    {
        if (heartRatesList.Count == 0)
        {
            return 0; // lub inna wartoœæ domyœlna
        }

        return (int)heartRatesList.Average();
    }

    public void ClearStatistics()
    {
        heartRatesList.Clear();
    }


}

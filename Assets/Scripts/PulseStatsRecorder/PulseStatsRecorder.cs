using System.Collections.Generic;
using UnityEngine;

public class PulseStatsRecorder : MonoBehaviour
{

    private List<float> pulseDataList = new List<float>();

    public void SavePulseData(float pulseData)
    {
        pulseDataList.Add(pulseData);
    }

    public List<float> GetPulseDataList()
    {
        return pulseDataList;
    }

    public void ClearPulseDataList()
    {
        pulseDataList.Clear();
    }

}
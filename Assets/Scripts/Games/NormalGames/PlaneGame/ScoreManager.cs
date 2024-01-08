using System.Collections;
using TMPro;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    [SerializeField] private TMP_Text scoreText;
    [SerializeField] private GameObject toolTip;
    private float score;
    private bool gameStarted = false;
    private IEnumerator Start()
    {
        // Wyszukaj obiekt tooltip


        // Czekaj, a¿ tooltip stanie siê nieaktywny
        while (toolTip.activeSelf)
        {
            yield return null;
        }


        // Rozpocznij grê
        gameStarted = true;

    }
    void Update()
    {
        if (!gameStarted)
        {
            return;
        }

        if (GameObject.FindGameObjectWithTag("Player") != null)
        {
            score += 1 * Time.deltaTime;
            scoreText.text = ((int)score).ToString();
        }
    }
}

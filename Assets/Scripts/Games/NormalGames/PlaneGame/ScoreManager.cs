using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Zenject.Asteroids;

public class ScoreManager : MonoBehaviour
{
    [SerializeField] private TMP_Text scoreText;
    [SerializeField] private GameObject toolTip;
    private float score;
    private bool gameStarted = false;
    private IEnumerator Start()
    {
        // Wyszukaj obiekt tooltip
      

        // Czekaj, a� tooltip stanie si� nieaktywny
        while (toolTip.activeSelf)
        {
            yield return null;
        }


        // Rozpocznij gr�
        gameStarted = true;

    }
    void Update()
    {
        if (!gameStarted)
        {
            return;
        }

        if (GameObject.FindGameObjectWithTag("Player")!= null)
        {
            score += 1* Time.deltaTime;
            scoreText.text = ((int)score).ToString();
        }
    }
}

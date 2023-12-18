using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    [SerializeField] private TMP_Text scoreText;
    private float score;

        void Update()
    {
      if(GameObject.FindGameObjectsWithTag("Player")!= null)
        {
            score += 1* Time.deltaTime;
            scoreText.text = ((int)score).ToString();
        }
    }
}

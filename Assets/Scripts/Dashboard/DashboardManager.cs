using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class DashboardManager : MonoBehaviour
{
    [SerializeField] private TMPro.TMP_Text levelText;
    [SerializeField] private TMPro.TMP_Text scoreText;
    [Inject] private LevelSystem levelSystem;
    [Inject] private SceneService sceneService;
    [Inject] private DialogSystem dialogSystem;

    public int numberOfLevels = 5;

    public string levelNamePrefix = "Level";
    public List<GameObject> levels = new List<GameObject>();

    public string lineNamePrefix = "Line";
    public List<GameObject> lines = new List<GameObject>();
    private void Start()
    {
        levelText.text = levelSystem.GetLevelName(levelSystem.Level);
        scoreText.text = levelSystem.Points.ToString();
        Progressbar();
    }
    public void onExitButton()
    {
        dialogSystem.ShowConfirmationDialog( "Are you sure you want to exit?",Back,null);
    }
    private void Back()
    {
        sceneService.LoadScene(sceneService.GetPrevScene());
    }
    private void GetProgressBarrObject()
    {
        for (int i = 1; i <= numberOfLevels; i++)
        {

            string levelObjectName = $"{levelNamePrefix}{i}";

            GameObject levelObject = GameObject.Find(levelObjectName);


            if (i > 1)
            {
                string lineObjectName = $"{lineNamePrefix}{i}";
                GameObject lineObject = GameObject.Find("Line").transform.Find(lineObjectName).gameObject;
                if (lineObject != null)
                    lines.Add(lineObject);
            }

            if (levelObject != null)
            {
                levels.Add(levelObject);

            }

        }
    }
    private void Progressbar()
    {
        GetProgressBarrObject();

        foreach (GameObject level in levels)
        {
            int levelIndex = levels.IndexOf(level) + 1;
            if (levelIndex >= levelSystem.Level)
            {
               
                level.transform.Find("Circle").GetComponent<Image>().color = new Color(0.7764707f, 0.5450981f, 0.2862745f);

            }
            else
            {

                level.transform.Find("Circle").GetComponent<Image>().color = new Color(0.3647059f, 0.2980392f, 0.227451f);
            }
            if (levelIndex == levelSystem.Level)
            {

                level.transform.Find("Icon").GetComponent<Image>().enabled = true;
            }

        }

        foreach (GameObject line in lines)
        {
            int levelIndex = lines.IndexOf(line) + 2;
            if (levelIndex > levelSystem.Level)
            {
                line.GetComponent<Image>().color = new Color(0.7764707f, 0.5450981f, 0.2862745f);
            }
            else
            {
                line.GetComponent<Image>().color = new Color(0.3647059f, 0.2980392f, 0.227451f);
            }
        }

    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class FourSevenEight : MonoBehaviour
{
    [Range(0, 100)]
    [SerializeField] private float fillValue = 0;
    [SerializeField] private Image circleFillImage;
    [Range(0, 100)]
    [SerializeField] private float fillValueBackground = 0;
    [SerializeField] private Image circleFillImageBackground;

    [SerializeField] private TMP_Text timeText;
    [SerializeField] private TMP_Text commandText;

    [Header("Objects")]
    [SerializeField] private GameObject inObject;
    [SerializeField] private GameObject outObject;
    [SerializeField] private GameObject hold1Object;


    private Color deactiveColor = new Color(0.2f, 0.23f, 0.35f);
    private Color activeColor = new Color(0.98f, 0.95f, 0.82f);


    private float fillTime = 19f; // czas potrzebny do napełnienia
    private float timer = 0f; // timer do śledzenia upływu czasu
    private bool isFilling = true; // flaga określająca, czy jesteśmy w trybie napełniania

    private int cycleCount = 0;

    private float counter;
    private bool isCounting = true;
    private TimeSpan timeSpan;

    private int timeIn = 4;
    private int timeOut = 8;
    private int timeHold = 7;

    bool isHold = false;
    bool isOut = false; 
    void Update()
    {
        if (cycleCount >= 8) // jeśli wykonaliśmy już 16 cykli, nie robimy nic więcej
        { 
            isCounting = false;
            return; }

        timer += Time.deltaTime; // zwiększamy timer o czas, który upłynął od ostatniej klatki

        if (isFilling)
        {
            fillValue = Mathf.Lerp(0, 100, timer / fillTime); // obliczamy nową wartość fillValue
            FillCircleValue(fillValue, circleFillImage);
            fillValue = 100;
        }
        else
        {
            fillValueBackground = Mathf.Lerp(0, 100, timer / fillTime); // obliczamy nową wartość fillValue
            FillCircleValue(fillValueBackground, circleFillImageBackground);
            fillValueBackground = 0;
        }



        // Jeśli timer przekroczył fillTime, zresetuj timer i zmień tryb
        if (timer >= fillTime)
        {
            timer = 0f;
            isFilling = !isFilling;
            cycleCount++;
            if (isFilling)
            {
                fillValue = 100;
                fillValueBackground = 0;
                circleFillImage.fillAmount = 0;
                circleFillImageBackground.fillAmount = 0;
            }
        }

        if (isCounting)
        {
            counter -= Time.deltaTime;
            timeSpan = TimeSpan.FromSeconds(counter);

            timeText.text = (timeSpan.Seconds + 1).ToString();
            if(counter <= 0 && isHold)
            {
                counter = timeOut;
                SwapBackgroundAndCommand(hold1Object, outObject, "Out");
                isOut = true;
                isHold = false;
            } 
            else if(counter <= 0 && isOut)
            {
                isOut = false;
                counter = timeIn;
                SwapBackgroundAndCommand(outObject, inObject, "In");
            }
            else if (counter <= 0)
            {
                counter = timeHold;
                isHold = true;
                SwapBackgroundAndCommand(inObject, hold1Object, "Hold");
            }
            
        }
    }
    private void Start()
    {
        commandText.text = "In";
        counter = timeIn;
        SetBackground(inObject, activeColor, true);
        SetBackground(outObject, deactiveColor, false);
        SetBackground(hold1Object, deactiveColor, false);
    }

    void FillCircleValue(float value, Image image)
    {
        float fillAmount = (value / 100.0f);
        image.fillAmount = fillAmount;
    }

    private void SwapBackgroundAndCommand(GameObject deactivateObject, GameObject activateObject, string command)
    {
        SetBackground(deactivateObject, deactiveColor, false);
        SetBackground(activateObject, activeColor, true);
        commandText.text = command;
    }


    private void SetBackground(GameObject gameObject, Color color, bool status)
    {
        var backGround = gameObject.transform.GetChild(0);
        backGround.gameObject.SetActive(status);
        var textCommand = gameObject.transform.GetChild(1).GetComponent<TMP_Text>();
        textCommand.color = color;
        var textTime = gameObject.transform.GetChild(2).GetComponent<TMP_Text>();
        textTime.color = color;
    }
}
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class YogaNose : MonoBehaviour
{
    [Range(0, 100)]
    [SerializeField] private float fillLeftHalf = 0;
    [SerializeField] private Image leftHalfNose;

    [Range(0, 100)]
    [SerializeField] private float fillRightHalf = 0;
    [SerializeField] private Image rightHalfNose;

    [SerializeField] private TMP_Text command;

    private float fillTime = 5.5f; // czas potrzebny do nape�nienia
    private float timer = 0f; // timer do �ledzenia up�ywu czasu
    private int phase = 0; // faza animacji

    [Header("Objects")]
    [SerializeField] private GameObject inObject;
    [SerializeField] private GameObject outObject;
    [SerializeField] private GameObject in1Object;
    [SerializeField] private GameObject out2Object;


    private Color deactiveColor = new Color(0.2f, 0.23f, 0.35f);
    private Color activeColor = new Color(0.98f, 0.95f, 0.82f);

    private int repetitions = 0;
    private void Start()
    {
        SetBackground(inObject, activeColor, true);
        SetBackground(outObject, deactiveColor, false);
        SetBackground(in1Object, deactiveColor, false);
        SetBackground(out2Object, deactiveColor, false);
    }
    void Update()
    {
        if (repetitions >= 7)
        {
            return;
        }

        timer += Time.deltaTime; // zwi�kszamy timer o czas, kt�ry up�yn�� od ostatniej klatki

        switch (phase)
        {
            case 0: // lewa strona zwi�ksza si� z 0 do 100
                leftHalfNose.fillOrigin = (int)Image.OriginVertical.Bottom;
                fillLeftHalf = Mathf.Lerp(0, 100, timer / fillTime);
                leftHalfNose.fillAmount = fillLeftHalf / 100.0f;
                SwapBackgroundAndCommand(out2Object, inObject, "In");
                break;
            case 1: // prawa strona zwi�ksza si� z 0 do 100
                rightHalfNose.fillOrigin = (int)Image.OriginVertical.Top;
                fillRightHalf = Mathf.Lerp(0, 100, timer / fillTime);
                rightHalfNose.fillAmount = fillRightHalf / 100.0f;
                SwapBackgroundAndCommand(inObject, outObject, "Out");
                break;
            case 2: // prawa strona zmienia fill origin na bottom i zwi�ksza si� z 0 do 100
                rightHalfNose.fillOrigin = (int)Image.OriginVertical.Bottom;
                fillRightHalf = Mathf.Lerp(0, 100, timer / fillTime);
                rightHalfNose.fillAmount = fillRightHalf / 100.0f;
                SwapBackgroundAndCommand(outObject, in1Object, "In");
                break;
            case 3: // lewa strona zmienia fill origin na top i zwi�ksza si� z 0 do 100
                leftHalfNose.fillOrigin = (int)Image.OriginVertical.Top;
                fillLeftHalf = Mathf.Lerp(0, 100, timer / fillTime);
                leftHalfNose.fillAmount = fillLeftHalf / 100.0f;
                SwapBackgroundAndCommand(in1Object, out2Object, "Out");
                break;
        }

        // Je�li timer przekroczy� fillTime, zresetuj timer i przejd� do nast�pnej fazy
        if (timer >= fillTime)
        {
            timer = 0f;
            phase = (phase + 1) % 4;
            if (phase == 0)
            {
                repetitions++;
            }
            leftHalfNose.fillAmount = 0;
            rightHalfNose.fillAmount = 0;
        }
    }
    private void SwapBackgroundAndCommand(GameObject deactivateObject, GameObject activateObject, string commandText)
    {
        SetBackground(deactivateObject, deactiveColor, false);
        SetBackground(activateObject, activeColor, true);
        command.text = commandText;
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
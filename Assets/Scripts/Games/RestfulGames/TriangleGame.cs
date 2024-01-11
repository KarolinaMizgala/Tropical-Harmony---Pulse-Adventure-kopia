using DG.Tweening;
using System;
using System.Collections;
using TMPro;
using UnityEngine;
using Zenject;

public class TriangleGame : MonoBehaviour
{
    [SerializeField] private GameObject circle;
    [SerializeField] private TMP_Text command;

    [SerializeField] private TMP_Text timeText;

    [Header("Objects")]
    [SerializeField] private GameObject inObject;
    [SerializeField] private GameObject outObject;
    [SerializeField] private GameObject hold1Object;

    [Inject] LevelSystem levelSystem;
    [Inject] DialogSystem dialogSystem;
    [Inject] SceneService sceneService;
    private Color deactiveColor = new Color(0.2f, 0.23f, 0.35f);
    private Color activeColor = new Color(0.98f, 0.95f, 0.82f);

    private float counter = 4f;
    private bool isCounting = false;
    TimeSpan timeSpan;

    private float timeIn = 5f;
    private float timeOut = 8f;
    private float timeHold = 5f;
    private bool isOut = false;
    private bool isHold = false;

    private IEnumerator Start()
    {
        var toolTip = GameObject.Find("Tooltip");
        while (toolTip.activeSelf && toolTip!=null)
        {
            yield return null;
        }

        // Odliczaj 3 sekundy
        for (int i = 3; i > 0; i--)
        {
            command.text = i.ToString();
            yield return new WaitForSeconds(1f);
        }

        // Rozpocznij grę
        FirtsRound();
        yield return null;
    }
    private void Update()
    {
        if (isCounting)
        {
            counter -= Time.deltaTime;
            timeSpan = TimeSpan.FromSeconds(counter);

            timeText.text = (timeSpan.Seconds + 1).ToString() + "s";

            if (counter <= 0 && isOut)
            {
                isOut = false;
                counter = 5f;
                isHold = true;

            }
            else if (counter <= 0 && isHold)
            {
                isHold = false;
                counter = 5f;
            }
            else if (counter <= 0 && !isOut && !isHold)
            {
                isOut = true;
                counter = 8f;
            }
        }
    }
    private void FirtsRound()
    {
        SetBackground(inObject, activeColor, true);
        SetBackground(outObject, deactiveColor, false);
        SetBackground(hold1Object, deactiveColor, false);

        command.text = "In";
        isCounting = true;
        Sequence sequence = DOTween.Sequence();
        for (int i = 0; i < 16; i++)
        {

            sequence.Append(circle.transform.DOLocalMove(new Vector3(330f, -230f, 0), timeIn)).AppendCallback(() =>
            {
                SwapBackgroundAndCommand(inObject, outObject, "Out");
            });
            sequence.Append(circle.transform.DOLocalMove(new Vector3(0f, 318, 0), timeOut)).AppendCallback(() =>
            {
                SwapBackgroundAndCommand(outObject, hold1Object, "Hold");
            });
            sequence.Append(circle.transform.DOLocalMove(new Vector3(-314f, -230.1f, 0), timeHold)).AppendCallback(() =>
            {
                SwapBackgroundAndCommand(hold1Object, inObject, "In");
            });


        }
        sequence.Append(circle.transform.DOLocalMove(new Vector3(330f, -230f, 0), timeIn)).AppendCallback(() =>
        {
            SwapBackgroundAndCommand(inObject, outObject, "Out");
        });
        sequence.Append(circle.transform.DOLocalMove(new Vector3(0f, 318, 0), timeOut)).AppendCallback(() =>
        {
            SwapBackgroundAndCommand(outObject, hold1Object, "Hold");
        });


        sequence.Append(circle.transform.DOLocalMove(new Vector3(-314f, -230.1f, 0), timeHold)).AppendCallback(() =>
        {
            isCounting = false;
            dialogSystem.ShowConfirmationDialog("Do you want to try again?", FirtsRound, Back);
            levelSystem.AddPoints(10);
        });

    }
    private void Back()
    {
        sceneService.LoadScene(SceneType.RestfulScene);
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

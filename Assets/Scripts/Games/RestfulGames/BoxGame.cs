using DG.Tweening;
using System;
using System.Collections;
using TMPro;
using UnityEngine;
using Zenject;

public class BoxGame : MonoBehaviour
{
    [SerializeField] private GameObject circle;
    [SerializeField] private TMP_Text command;

    [SerializeField] private TMP_Text timeText;

    [Header("Objects")]
    [SerializeField] private GameObject inObject;
    [SerializeField] private GameObject outObject;
    [SerializeField] private GameObject hold1Object;
    [SerializeField] private GameObject hold2Object;

    [Inject] LevelSystem levelSystem;
    [Inject] DialogSystem dialogSystem;
    [Inject] SceneService sceneService;
    private Color deactiveColor = new Color(0.2f, 0.23f, 0.35f);
    private Color activeColor = new Color(0.98f, 0.95f, 0.82f);

    private float counter = 4f;
    private bool isCounting = false;
    TimeSpan timeSpan;

    private float time = 4f;

    private IEnumerator Start()
    {
        var toolTip = GameObject.Find("Tooltip");
        while (toolTip.activeSelf)
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
            if (counter <= 0)
            {
                counter = 4f;
            }
        }
    }
    private void FirtsRound()
    {
        SetBackground(inObject, activeColor, true);
        SetBackground(outObject, deactiveColor, false);
        SetBackground(hold1Object, deactiveColor, false);
        SetBackground(hold2Object, deactiveColor, false);

        command.text = "In";
        isCounting = true;
        Sequence sequence = DOTween.Sequence();
        for (int i = 0; i < 15; i++)
        {
            sequence.Append(circle.transform.DOLocalMove(new Vector3(-197.5f, 197.5f, 0), time)).AppendCallback(() =>
            {
                SwapBackgroundAndCommand(inObject, hold1Object, "Hold");
            });
            sequence.Append(circle.transform.DOLocalMove(new Vector3(197.1f, 197.1f, 0), time)).AppendCallback(() =>
            {
                SwapBackgroundAndCommand(hold1Object, outObject, "Out");
            });
            sequence.Append(circle.transform.DOLocalMove(new Vector3(197.1f, -197.1f, 0), time)).AppendCallback(() =>
            {
                SwapBackgroundAndCommand(outObject, hold2Object, "Hold");
            });
            sequence.Append(circle.transform.DOLocalMove(new Vector3(-197.1f, -197.1f, 0), time)).AppendCallback(() =>
            {
                SwapBackgroundAndCommand(hold2Object, inObject, "In");
            });

        }
        sequence.Append(circle.transform.DOLocalMove(new Vector3(-197.5f, 197.5f, 0), time)).AppendCallback(() =>
        {
            SwapBackgroundAndCommand(inObject, hold1Object, "Hold");
        });
        sequence.Append(circle.transform.DOLocalMove(new Vector3(197.1f, 197.1f, 0), time)).AppendCallback(() =>
        {
            SwapBackgroundAndCommand(hold1Object, outObject, "Out");
        });
        sequence.Append(circle.transform.DOLocalMove(new Vector3(197.1f, -197.1f, 0), time)).AppendCallback(() =>
        {
            SwapBackgroundAndCommand(outObject, hold2Object, "Hold");
        });
        sequence.Append(circle.transform.DOLocalMove(new Vector3(-197.1f, -197.1f, 0), time)).AppendCallback(() =>
        {
            isCounting = false;
            dialogSystem.ShowConfirmationDialog("Do you want to try again?", FirtsRound, BackLastScene);
            levelSystem.AddPoints(10);
        });

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
    private void BackLastScene()
    {
        sceneService.LoadScene(SceneType.RestfulScene);
    }
}

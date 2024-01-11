using DG.Tweening;
using System;
using System.Collections;
using TMPro;
using UnityEngine;
using Zenject;

/// <summary>
/// Manages the logic for the TriangleGame, a mini-game involving object movements.
/// </summary>
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
    private GameObject toolTip;

    /// <summary>
    /// Coroutine that initializes the game and starts the first round.
    /// </summary>
    private IEnumerator Start()
    {
        toolTip = GameObject.Find("Tooltip");

        while (toolTip.activeSelf)
        {
            yield return null;
        }

        // Countdown for 3 seconds
        for (int i = 3; i > 0; i--)
        {
            command.text = i.ToString();
            yield return new WaitForSeconds(1f);
        }

        // Start the game
        FirtsRound();
        yield return null;
    }

    /// <summary>
    /// Update is called once per frame.
    /// </summary>
    private void Update()
    {
        if (toolTip.activeSelf || dialogSystem.IsDialogVisible())
        {
            Time.timeScale = 0;
        }
        else
        {
            Time.timeScale = 1;
        }
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

    /// <summary>
    /// Initiates the first round of the game.
    /// </summary>
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

    /// <summary>
    /// Switches background and updates the command text.
    /// </summary>
    private void SwapBackgroundAndCommand(GameObject deactivateObject, GameObject activateObject, string commandText)
    {
        SetBackground(deactivateObject, deactiveColor, false);
        SetBackground(activateObject, activeColor, true);
        command.text = commandText;
    }

    /// <summary>
    /// Sets the background of the specified object with the given color and status.
    /// </summary>
    private void SetBackground(GameObject gameObject, Color color, bool status)
    {
        var backGround = gameObject.transform.GetChild(0);
        backGround.gameObject.SetActive(status);
        var textCommand = gameObject.transform.GetChild(1).GetComponent<TMP_Text>();
        textCommand.color = color;
        var textTime = gameObject.transform.GetChild(2).GetComponent<TMP_Text>();
        textTime.color = color;
    }

    /// <summary>
    /// Loads the previous scene.
    /// </summary>
    private void Back()
    {
        sceneService.LoadScene(SceneType.RestfulScene);
    }
}

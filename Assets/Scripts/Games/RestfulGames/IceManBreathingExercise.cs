using DG.Tweening;
using System;
using System.Collections;
using TMPro;
using UnityEngine;
using Zenject;

/// <summary>
/// Manages the Ice Man Breathing Exercise game logic.
/// </summary>
public class IceManBreathingExercise : MonoBehaviour
{
    [SerializeField] private GameObject circle;
    [SerializeField] private TMP_Text command;

    [SerializeField] private GameObject TextContainer;
    [SerializeField] private GameObject TipText;

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

    private float timeIn = 1.4f;
    private float timeOut = 1.6f;
    private float timeHold1 = 15f;
    private float timeHold2 = 3;

    private float counter;
    private bool isCounting = false;
    TimeSpan timeSpan;

    private GameObject toolTip;

    /// <summary>
    /// Coroutine that initializes the Ice Man Breathing Exercise.
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
    /// Updates the game state based on user input and UI visibility.
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

        if (Input.GetKeyDown(KeyCode.Space))
        {
            isCounting = false;
            StopCounting();
        }

        if (isCounting)
        {
            counter += Time.deltaTime;
            timeSpan = TimeSpan.FromSeconds(counter);

            command.text = string.Format("{0:0}:{1:00}", timeSpan.Minutes, timeSpan.Seconds);
        }
    }

    /// <summary>
    /// Initiates the first round of the breathing exercise.
    /// </summary>
    private void FirtsRound()
    {
        DisableObject();
        timeIn = 1.4f;
        timeOut = 1.6f;

        SetObjectProperties(inObject, timeIn, activeColor, true);
        SetObjectProperties(outObject, timeOut, deactiveColor, false);

        command.text = "In";

        Sequence sequence = DOTween.Sequence();
        for (int i = 0; i < 2; i++)
        {
            sequence.Append(circle.transform.DOScale(1.9f, timeIn)).AppendCallback(() =>
            {
                SwapBackgroundAndCommand(inObject, outObject, "Out");
            });
            sequence.Append(circle.transform.DOScale(1f, timeOut)).AppendCallback(() =>
            {
                SwapBackgroundAndCommand(outObject, inObject, "In");
            });
        }
        sequence.Append(circle.transform.DOScale(1.9f, timeIn)).AppendCallback(() =>
        {
            SwapBackgroundAndCommand(inObject, outObject, "Out");
        });
        sequence.Append(circle.transform.DOScale(1f, timeOut)).AppendCallback(() =>
        {
            SwapBackgroundAndCommand(outObject, inObject, "In");
        }).OnComplete(() => SecondRound());
    }

    /// <summary>
    /// Initiates the second round of the breathing exercise.
    /// </summary>
    private void SecondRound()
    {
        TipText.SetActive(true);
        TextContainer.SetActive(false);
        StartCounting();
    }

    /// <summary>
    /// Starts the countdown timer.
    /// </summary>
    public void StartCounting()
    {
        isCounting = true;
    }

    /// <summary>
    /// Stops the countdown timer and proceeds to the third round.
    /// </summary>
    public void StopCounting()
    {
        isCounting = false;
        ThirdRound();
    }

    /// <summary>
    /// Handles the logic for the third round of the breathing exercise.
    /// </summary>
    private void ThirdRound()
    {
        TipText.SetActive(false);
        TextContainer.SetActive(true);

        EnableObject();

        timeIn = 2f;
        timeOut = 2f;
        timeHold1 = 15f;
        timeHold2 = 3;

        SetObjectProperties(inObject, timeIn, activeColor, true);
        SetObjectProperties(outObject, timeOut, deactiveColor, false);
        SetObjectProperties(hold1Object, timeHold1, deactiveColor, false);
        SetObjectProperties(hold2Object, timeHold2, deactiveColor, false);

        command.text = "In";

        Sequence sequence = DOTween.Sequence();

        sequence.Append(circle.transform.DOScale(1.9f, timeIn)).AppendCallback(() =>
        {
            SwapBackgroundAndCommand(inObject, hold1Object, "Hold");
        });
        sequence.Append(circle.transform.DOScale(1.9f, timeHold1)).AppendCallback(() =>
        {
            SwapBackgroundAndCommand(hold1Object, outObject, "Out");
        });
        sequence.Append(circle.transform.DOScale(1f, timeOut)).AppendCallback(() =>
        {
            SwapBackgroundAndCommand(outObject, hold2Object, "Hold");
        });
        sequence.Append(circle.transform.DOScale(1f, timeHold2)).AppendCallback(() =>
        {
            dialogSystem.ShowConfirmationDialog("Do you want to try again?", FirtsRound, Back);
            levelSystem.AddPoints(10);
        });
    }

    /// <summary>
    /// Sets the properties of the given object.
    /// </summary>
    private void SetObjectProperties(GameObject gameObject, float time, Color color, bool status)
    {
        SetTime(gameObject, time);
        SetBackground(gameObject, color, status);
    }

    /// <summary>
    /// Disables the hold objects.
    /// </summary>
    private void DisableObject()
    {
        hold1Object.SetActive(false);
        hold2Object.SetActive(false);
    }

    /// <summary>
    /// Enables the hold objects.
    /// </summary>
    private void EnableObject()
    {
        hold1Object.SetActive(true);
        hold2Object.SetActive(true);
    }

    /// <summary>
    /// Sets the background properties of the given object.
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
    /// Sets the time text for the given object.
    /// </summary>
    private void SetTime(GameObject gameObject, float time)
    {
        var textTime = gameObject.transform.GetChild(2).GetComponent<TMP_Text>();
        textTime.text = time.ToString() + "s";
    }

    /// <summary>
    /// Swaps the background and updates the command text for the given objects.
    /// </summary>
    private void SwapBackgroundAndCommand(GameObject deactivateObject, GameObject activateObject, string commandText)
    {
        SetBackground(deactivateObject, deactiveColor, false);
        SetBackground(activateObject, activeColor, true);
        command.text = commandText;
    }

    /// <summary>
    /// Navigates back to the restful scene.
    /// </summary>
    void Back()
    {
        sceneService.LoadScene(SceneType.RestfulScene);
    }
}

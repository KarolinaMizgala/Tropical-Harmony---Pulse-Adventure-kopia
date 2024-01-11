using DG.Tweening;
using System;
using System.Collections;
using TMPro;
using UnityEngine;
using Zenject;

/// <summary>
/// Manages the logic for the BoxGame, a mini-game involving object movements.
/// </summary>
public class BoxGame : MonoBehaviour
{
    [SerializeField] private GameObject circle; // Reference to the circle object.
    [SerializeField] private TMP_Text command;  // UI text for displaying commands.
    [SerializeField] private TMP_Text timeText; // UI text for displaying time.

    [Header("Objects")]
    [SerializeField] private GameObject inObject;    // In object.
    [SerializeField] private GameObject outObject;   // Out object.
    [SerializeField] private GameObject hold1Object; // Hold1 object.
    [SerializeField] private GameObject hold2Object; // Hold2 object.

    [Inject] LevelSystem levelSystem;      // Reference to the LevelSystem.
    [Inject] DialogSystem dialogSystem;    // Reference to the DialogSystem.
    [Inject] SceneService sceneService;    // Reference to the SceneService.

    private Color deactiveColor = new Color(0.2f, 0.23f, 0.35f); // Color for deactivated objects.
    private Color activeColor = new Color(0.98f, 0.95f, 0.82f);  // Color for activated objects.

    private float counter = 4f;     // Counter for time-related operations.
    private bool isCounting = false; // Flag indicating whether the countdown is active.
    private TimeSpan timeSpan;       // TimeSpan used for displaying time.
    GameObject toolTip;               // Reference to the tooltip object.
    private float time = 4f;         // Default time value.

    /// <summary>
    /// Coroutine that initializes the game and starts the first round.
    /// </summary>
    private IEnumerator Start()
    {
        toolTip = GameObject.Find("Tooltip");

        // Wait until the tooltip is not active.
        while (toolTip.activeSelf)
        {
            yield return null;
        }

        // Countdown for 3 seconds.
        for (int i = 3; i > 0; i--)
        {
            command.text = i.ToString();
            yield return new WaitForSeconds(1f);
        }

        // Start the first round of the game.
        FirtsRound();
        yield return null;
    }

    /// <summary>
    /// Update is called once per frame.
    /// </summary>
    private void Update()
    {
        // Pause the game if the tooltip is active.
        if (toolTip.activeSelf || dialogSystem.IsDialogVisible())
        {
            Time.timeScale = 0;
        }
        else
        {
            Time.timeScale = 1;
        }

        // Update the countdown timer.
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

    /// <summary>
    /// Initiates the first round of the game.
    /// </summary>
    private void FirtsRound()
    {
        // Set initial backgrounds and commands.
        SetBackground(inObject, activeColor, true);
        SetBackground(outObject, deactiveColor, false);
        SetBackground(hold1Object, deactiveColor, false);
        SetBackground(hold2Object, deactiveColor, false);

        command.text = "In";
        isCounting = true;

        // Create a sequence of circle movements.
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

        // Final movements and actions.
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

    /// <summary>
    /// Swaps backgrounds and updates the command text.
    /// </summary>
    /// <param name="deactivateObject">Object to deactivate.</param>
    /// <param name="activateObject">Object to activate.</param>
    /// <param name="commandText">New command text.</param>
    private void SwapBackgroundAndCommand(GameObject deactivateObject, GameObject activateObject, string commandText)
    {
        SetBackground(deactivateObject, deactiveColor, false);
        SetBackground(activateObject, activeColor, true);
        command.text = commandText;
    }

    /// <summary>
    /// Sets the background of the specified object with the given color and status.
    /// </summary>
    /// <param name="gameObject">Object whose background to set.</param>
    /// <param name="color">Color to set.</param>
    /// <param name="status">Status indicating whether to activate the background.</param>
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
    private void BackLastScene()
    {
        sceneService.LoadScene(SceneType.RestfulScene);
    }
}

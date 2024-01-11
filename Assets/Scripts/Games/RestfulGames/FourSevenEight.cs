using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

/// <summary>
/// Manages the logic for the "FourSevenEight" game, involving time-based fills and object interactions.
/// </summary>
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

    [Inject] LevelSystem levelSystem;
    [Inject] DialogSystem dialogSystem;
    [Inject] SceneService sceneService;

    private Color deactiveColor = new Color(0.2f, 0.23f, 0.35f);
    private Color activeColor = new Color(0.98f, 0.95f, 0.82f);

    private float fillTime = 19f; // Time needed to fill
    private float timer = 0f;    // Timer to track elapsed time
    private bool isFilling = true; // Flag indicating whether filling is in progress

    private int cycleCount = 0;

    private float counter;
    private bool isCounting = true;
    private TimeSpan timeSpan;

    private int timeIn = 4;
    private int timeOut = 8;
    private int timeHold = 7;

    bool isHold = false;
    bool isOut = false;

    private bool gameStarted = false;

    private GameObject toolTip;

    /// <summary>
    /// Coroutine that initializes the game and starts the first round.
    /// </summary>
    private IEnumerator Start()
    {
        // Find the tooltip object.
        toolTip = GameObject.Find("Tooltip");

        // Wait until the tooltip becomes inactive.
        while (toolTip.activeSelf)
        {
            yield return null;
        }

        // Countdown for 3 seconds.
        for (int i = 3; i > 0; i--)
        {
            commandText.text = i.ToString();
            yield return new WaitForSeconds(1f);
        }

        // Start the game.
        gameStarted = true;
        commandText.text = "In";
        counter = timeIn;
        SetBackground(inObject, activeColor, true);
        SetBackground(outObject, deactiveColor, false);
        SetBackground(hold1Object, deactiveColor, false);
    }

    /// <summary>
    /// Update is called once per frame.
    /// </summary>
    void Update()
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

        if (!gameStarted)
        {
            return;
        }

        // Main game logic.
        LogicGame();
    }

    /// <summary>
    /// Restarts the game.
    /// </summary>
    void Restart()
    {
        cycleCount = 0;
        isCounting = true;
        levelSystem.AddPoints(10);
        LogicGame();
    }

    /// <summary>
    /// Main logic for the game.
    /// </summary>
    void LogicGame()
    {
        // If we have completed 16 cycles, show a dialog and end the game.
        if (cycleCount >= 8)
        {
            isCounting = false;
            dialogSystem.ShowConfirmationDialog("Do you want to try again?", Restart, Back);
            return;
        }

        // Increase the timer by the time elapsed since the last frame.
        timer += Time.deltaTime;

        // Update fill values based on the timer and filling mode.
        if (isFilling)
        {
            fillValue = Mathf.Lerp(0, 100, timer / fillTime);
            FillCircleValue(fillValue, circleFillImage);
            fillValue = 100;
        }
        else
        {
            fillValueBackground = Mathf.Lerp(0, 100, timer / fillTime);
            FillCircleValue(fillValueBackground, circleFillImageBackground);
            fillValueBackground = 0;
        }

        // If the timer exceeds fillTime, reset the timer and change the filling mode.
        if (timer >= fillTime)
        {
            timer = 0f;
            isFilling = !isFilling;
            cycleCount++;

            // Reset fill values and fill amounts when changing modes.
            if (isFilling)
            {
                fillValue = 100;
                fillValueBackground = 0;
                circleFillImage.fillAmount = 0;
                circleFillImageBackground.fillAmount = 0;
            }
        }

        // Update the countdown timer.
        if (isCounting)
        {
            counter -= Time.deltaTime;
            timeSpan = TimeSpan.FromSeconds(counter);

            timeText.text = (timeSpan.Seconds + 1).ToString();

            // Logic for changing commands based on timer values.
            if (counter <= 0 && isHold)
            {
                counter = timeOut;
                SwapBackgroundAndCommand(hold1Object, outObject, "Out");
                isOut = true;
                isHold = false;
            }
            else if (counter <= 0 && isOut)
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

    /// <summary>
    /// Fills the given image's fill amount based on the provided value.
    /// </summary>
    /// <param name="value">Fill value (percentage).</param>
    /// <param name="image">Image to fill.</param>
    void FillCircleValue(float value, Image image)
    {
        float fillAmount = (value / 100.0f);
        image.fillAmount = fillAmount;
    }

    /// <summary>
    /// Swaps backgrounds and updates the command text.
    /// </summary>
    /// <param name="deactivateObject">Object to deactivate.</param>
    /// <param name="activateObject">Object to activate.</param>
    /// <param name="command">New command text.</param>
    private void SwapBackgroundAndCommand(GameObject deactivateObject, GameObject activateObject, string command)
    {
        SetBackground(deactivateObject, deactiveColor, false);
        SetBackground(activateObject, activeColor, true);
        commandText.text = command;
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
    private void Back()
    {
        levelSystem.AddPoints(10);
        sceneService.LoadScene(SceneType.RestfulScene);
    }
}

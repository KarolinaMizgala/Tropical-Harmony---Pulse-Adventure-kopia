using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

/// <summary>
/// Manages the YogaNose mini-game, which involves filling two halves of an image alternately.
/// </summary>
public class YogaNose : MonoBehaviour
{
    [Range(0, 100)]
    [SerializeField] private float fillLeftHalf = 0;
    [SerializeField] private Image leftHalfNose;

    [Range(0, 100)]
    [SerializeField] private float fillRightHalf = 0;
    [SerializeField] private Image rightHalfNose;

    [SerializeField] private TMP_Text command;

    private float fillTime = 5.5f; // Time needed to fill
    private float timer = 0f; // Timer to track elapsed time
    private int phase = 0; // Animation phase

    [Header("Objects")]
    [SerializeField] private GameObject inObject;
    [SerializeField] private GameObject outObject;
    [SerializeField] private GameObject in1Object;
    [SerializeField] private GameObject out2Object;

    [Inject] LevelSystem levelSystem;
    [Inject] DialogSystem dialogSystem;
    [Inject] SceneService sceneService;

    private Color deactiveColor = new Color(0.2f, 0.23f, 0.35f);
    private Color activeColor = new Color(0.98f, 0.95f, 0.82f);
    private bool gameStarted = false;
    private int repetitions = 0;

    private GameObject toolTip;

    /// <summary>
    /// Coroutine that initializes the game and starts the first round.
    /// </summary>
    private IEnumerator Start()
    {
        SetBackground(inObject, activeColor, true);
        SetBackground(outObject, deactiveColor, false);
        SetBackground(in1Object, deactiveColor, false);
        SetBackground(out2Object, deactiveColor, false);
        toolTip = GameObject.Find("Tooltip");

        // Wait until the tooltip becomes inactive
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
        repetitions = 0;

        // Start the game
        gameStarted = true;
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
        if (!gameStarted)
        {
            return;
        }

        GameLogic();
    }

    /// <summary>
    /// Restart the game.
    /// </summary>
    private void Restart()
    {
        repetitions = 0;
        levelSystem.AddPoints(10);
        GameLogic();
    }

    /// <summary>
    /// Main logic for the YogaNose game.
    /// </summary>
    private void GameLogic()
    {
        if (repetitions >= 28)
        {
            dialogSystem.ShowConfirmationDialog("Do you want to try again?", Restart, Back);
            return;
        }

        timer += Time.deltaTime; // Increase the timer by the time that has passed since the last frame

        switch (phase)
        {
            case 0: // Left side increases from 0 to 100
                leftHalfNose.fillOrigin = (int)Image.OriginVertical.Bottom;
                fillLeftHalf = Mathf.Lerp(0, 100, timer / fillTime);
                leftHalfNose.fillAmount = fillLeftHalf / 100.0f;
                SwapBackgroundAndCommand(out2Object, inObject, "In");
                break;
            case 1: // Right side increases from 0 to 100
                rightHalfNose.fillOrigin = (int)Image.OriginVertical.Top;
                fillRightHalf = Mathf.Lerp(0, 100, timer / fillTime);
                rightHalfNose.fillAmount = fillRightHalf / 100.0f;
                SwapBackgroundAndCommand(inObject, outObject, "Out");
                break;
            case 2: // Right side changes fill origin to bottom and increases from 0 to 100
                rightHalfNose.fillOrigin = (int)Image.OriginVertical.Bottom;
                fillRightHalf = Mathf.Lerp(0, 100, timer / fillTime);
                rightHalfNose.fillAmount = fillRightHalf / 100.0f;
                SwapBackgroundAndCommand(outObject, in1Object, "In");
                break;
            case 3: // Left side changes fill origin to top and increases from 0 to 100
                leftHalfNose.fillOrigin = (int)Image.OriginVertical.Top;
                fillLeftHalf = Mathf.Lerp(0, 100, timer / fillTime);
                leftHalfNose.fillAmount = fillLeftHalf / 100.0f;
                SwapBackgroundAndCommand(in1Object, out2Object, "Out");
                break;
        }

        // If the timer exceeds fillTime, reset the timer and move to the next phase
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

    /// <summary>
    /// Swap background and update command text.
    /// </summary>
    private void SwapBackgroundAndCommand(GameObject deactivateObject, GameObject activateObject, string commandText)
    {
        SetBackground(deactivateObject, deactiveColor, false);
        SetBackground(activateObject, activeColor, true);
        command.text = commandText;
    }

    /// <summary>
    /// Set background color and activate/deactivate the object.
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
    /// Navigate back to the main scene.
    /// </summary>
    private void Back()
    {
        levelSystem.AddPoints(10);
        sceneService.LoadScene(SceneType.RestfulScene);
    }
}

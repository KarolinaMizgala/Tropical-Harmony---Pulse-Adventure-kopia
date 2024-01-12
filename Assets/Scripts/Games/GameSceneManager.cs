using UnityEngine;
using Zenject;

/// <summary>
/// Manages the game scenes.
/// </summary>
public class GameSceneManager : MonoBehaviour
{
    [SerializeField] private SceneType prevScene;
    [SerializeField] private SceneType scene;
    [SerializeField] private GameObject tooltip;

    public bool isActive = false;

    [Inject] SceneService sceneService;
    [Inject] DialogSystem dialogSystem;

    private bool tooltipShown;

    /// <summary>
    /// Initializes the game scene.
    /// </summary>
    public void Start()
    {
        tooltipShown = PlayerPrefs.GetInt("TooltipShown_" + scene.ToString(), 0) == 1;

        // If the tooltip has not been shown, call the method to show the tooltip
        tooltip.SetActive(!tooltipShown);
    }

    /// <summary>
    /// Hides the tooltip and sets the tooltip shown flag.
    /// </summary>
    public void onExitTooltip()
    {
        PlayerPrefs.SetInt("TooltipShown_" + scene, 1);
        tooltip.SetActive(false);
    }

    /// <summary>
    /// Shows a confirmation dialog when the exit button is clicked.
    /// </summary>
    public void OnExit()
    {
        dialogSystem.ShowConfirmationDialog("Your points will not be awarded upon exit. Are you sure you want to return to the main gameplay?", BackLastScene, null);
    }

    /// <summary>
    /// Loads the previous scene.
    /// </summary>
    private void BackLastScene()
    {
        sceneService.LoadScene(prevScene);
    }

    /// <summary>
    /// Shows the tooltip when the question button is clicked.
    /// </summary>
    public void OnQuestionClick()
    {
        isActive = true;
        tooltip.SetActive(true);
    }
}
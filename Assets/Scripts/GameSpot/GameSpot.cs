using UnityEngine;
using Zenject;

/// <summary>
/// Manages the game spots in the game.
/// </summary>
public class GameSpot : MonoBehaviour
{
    [Inject] SceneService sceneService;
    [Inject] DialogSystem dialogSystem;
    [SerializeField] private SceneType sceneType;
    private bool isDialogShown = false;
    private SceneType scene;
    [SerializeField] private string text = "You've reached the dashboard. Are you ready for a scene change?";
    private string dialogShownKey;

    /// <summary>
    /// Initializes the game spot.
    /// </summary>
    private void Start()
    {
        dialogShownKey = "DialogShown_" + sceneType.ToString();
        // Check if the dialog has already been shown for the scene
        isDialogShown = PlayerPrefs.GetInt(dialogShownKey, 0) == 1;
    }

    /// <summary>
    /// Shows a confirmation dialog when the player stays in the collision.
    /// </summary>
    private void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            if (!isDialogShown)
            {
                isDialogShown = true;
                PlayerPrefs.SetInt(dialogShownKey, 1);
                dialogSystem.ShowConfirmationDialog(text, OnYesClick, null);
            }
        }
    }

    /// <summary>
    /// Resets the dialog shown flag when the player exits the collision.
    /// </summary>
    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            isDialogShown = false;
        }
    }

    /// <summary>
    /// Loads the scene when the yes button is clicked.
    /// </summary>
    private void OnYesClick()
    {
        sceneService.SetPrevScene(sceneService.GetActivScene());
        sceneService.LoadScene(sceneType);
    }

    /// <summary>
    /// Returns the scene type.
    /// </summary>
    public SceneType GetSceneType()
    {
        return sceneType;
    }
}
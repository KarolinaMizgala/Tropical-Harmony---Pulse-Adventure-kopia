using UnityEngine;
using UnityEngine.SceneManagement;
using Zenject;

/// <summary>
/// Handles the game over state.
/// </summary>
public class GameOver : MonoBehaviour
{
    /// <summary>
    /// The dialog system.
    /// </summary>
    [Inject] DialogSystem dialogSystem;

    /// <summary>
    /// The scene service.
    /// </summary>
    [Inject] SceneService sceneService;

    /// <summary>
    /// The level system.
    /// </summary>
    [Inject] LevelSystem levelSystem;

    /// <summary>
    /// Indicates whether a dialog is currently showing.
    /// </summary>
    bool isDialogShowing = false;

    /// <summary>
    /// The game time.
    /// </summary>
    private float gameTime = 0f;

    /// <summary>
    /// Updates the game time every frame and checks if the player is null.
    /// </summary>
    void Update()
    {
        gameTime += Time.deltaTime;
        if (GameObject.FindGameObjectWithTag("Player") == null)
        {
            if (!isDialogShowing)
            {
                isDialogShowing = true;
                if (gameTime > 2f)
                {
                    levelSystem.AddPoints(5);
                }
                dialogSystem.ShowConfirmationDialog("You lost. Do you want to try again?", Restart, Back);
            }
        }
    }

    /// <summary>
    /// Restarts the current scene.
    /// </summary>
    private void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        isDialogShowing = false;
    }

    /// <summary>
    /// Goes back to the energetic scene.
    /// </summary>
    private void Back()
    {
        isDialogShowing = false;
        sceneService.LoadScene(SceneType.Energetic);
    }
}
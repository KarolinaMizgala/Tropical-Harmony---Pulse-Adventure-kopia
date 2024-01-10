using UnityEngine;
using UnityEngine.SceneManagement;
using Zenject;

public class GameOver : MonoBehaviour
{
    [Inject] DialogSystem dialogSystem;
    [Inject] SceneService sceneService;
    [Inject] LevelSystem levelSystem;
    bool isDialogShowing = false;
    private float gameTime = 0f;
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
    private void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        isDialogShowing = false;
    }

    private void Back()
    {
        isDialogShowing = false;
        sceneService.LoadScene(SceneType.Energetic);
    }
}

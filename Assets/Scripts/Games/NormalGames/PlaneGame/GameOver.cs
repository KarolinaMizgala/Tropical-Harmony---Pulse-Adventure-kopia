using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Zenject;

public class GameOver : MonoBehaviour
{
    [Inject] DialogSystem dialogSystem;
    [Inject] SceneService sceneService;
    bool isDialogShowing = false;

    void Update()
    {
        if(GameObject.FindGameObjectWithTag("Player")== null)
        {
            if(!isDialogShowing)
            {
                isDialogShowing = true;
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

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Zenject;

public class GameOver : MonoBehaviour
{
    [Inject] DialogSystem dialogSystem;
    [Inject] SceneService sceneService;

    void Update()
    {
        if(GameObject.FindGameObjectsWithTag("Player")== null)
        {
            dialogSystem.ShowConfirmationDialog("You lost. Do you want to try again?", Restart, Back);
        }
    }
    private void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    private void Back()
    {
        sceneService.LoadScene(SceneType.Energetic);
    }
}

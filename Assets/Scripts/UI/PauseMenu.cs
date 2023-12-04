using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class PauseMenu : MonoBehaviour
{
    [Inject] SceneService sceneService;

    [SerializeField] private GameObject muteButton;
    [SerializeField] private GameObject unmuteButton;

    public void onResumeClick()
    {
        gameObject.SetActive(false);
    }
    public void onMuteClick()
    {
        if (muteButton.activeSelf)
        {
            muteButton.SetActive(false);
            unmuteButton.SetActive(true);
        }
        else
        {
            muteButton.SetActive(true);
            unmuteButton.SetActive(false);
        }
    }
    public void onMenuClick()
    {
        sceneService.LoadScene(SceneType.MainMenu);
    }
    public void onExitClick()
    {
        Application.Quit();
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class GameSpot : MonoBehaviour
{
    [Inject] SceneService sceneService; 
    [Inject] DialogSystem dialogSystem;
    [SerializeField] private SceneType sceneType;
    private bool isDialogShown = false;
    // Start is called before the first frame update
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            if(!isDialogShown)
            {
                isDialogShown = true;
                dialogSystem.ShowConfirmationDialog("You've reached the mini-game area. Are you ready for a scene change?", OnYesClick, null);
            }
           
           
        }
    }
    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            isDialogShown = false;
        }
    }
    private void OnYesClick()
    {
        sceneService.LoadScene(sceneType);
    }

  
    
}

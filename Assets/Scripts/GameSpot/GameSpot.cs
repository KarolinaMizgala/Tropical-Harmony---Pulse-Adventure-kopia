using UnityEngine;
using Zenject;

public class GameSpot : MonoBehaviour
{
    [Inject] SceneService sceneService;
    [Inject] DialogSystem dialogSystem;
    [SerializeField] private SceneType sceneType;
    private bool isDialogShown = false;
    private SceneType scene;
    [SerializeField] private string text = "You've reached the mini-game area. Are you ready for a scene change?";
    private string dialogShownKey;
    private void Start()
    {
        dialogShownKey = "DialogShown_" + sceneType.ToString();
        // Sprawdzamy, czy dialog by³ ju¿ pokazany dla danej sceny
        isDialogShown = PlayerPrefs.GetInt(dialogShownKey, 0) == 1;
    }
    // Start is called before the first frame update
    private void OnCollisionEnter(Collision collision)
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
    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            isDialogShown = false;
        }
    }
    private void OnYesClick()
    {
        sceneService.SetPrevScene(sceneService.GetActivScene());
        sceneService.LoadScene(sceneType);
    }

    public SceneType GetSceneType()
    {
        return sceneType;
    }


}

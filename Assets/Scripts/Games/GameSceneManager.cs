using UnityEngine;
using Zenject;

public class GameSceneManager : MonoBehaviour
{
    [SerializeField] private SceneType prevScene;
    [SerializeField] private SceneType scene;
    [SerializeField] private GameObject tooltip;

    public bool isActive = false;

    [Inject] SceneService sceneService;
    [Inject] DialogSystem dialogSystem;

    private bool tooltipShown;

    public void Start()
    {
        tooltipShown = PlayerPrefs.GetInt("TooltipShown_" + scene.ToString(), 0) == 1;

        // Jeśli tooltip nie został wyświetlony, wywołaj metodę wyświetlającą tooltip
        tooltip.SetActive(!tooltipShown);
    }

    public void onExitTooltip()
    {
        PlayerPrefs.SetInt("TooltipShown_" + scene, 1);
        tooltip.SetActive(false);
    }

    public void OnExit()
    {
        dialogSystem.ShowConfirmationDialog("Your points will not be awarded upon exit. Are you sure you want to return to the main gameplay?", BackLastScene, null);


    }
    private void BackLastScene()
    {
        sceneService.LoadScene(prevScene);
    }
    public void OnQuestionClick()
    {
        isActive = true;
        tooltip.SetActive(true);
    }
}

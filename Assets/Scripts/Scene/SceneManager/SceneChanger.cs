using UnityEngine;
using UnityEngine.SceneManagement;
using Zenject;

public class SceneChanger : MonoBehaviour
{
    [Inject] private SceneService sceneService;
    public void ChangeRestfulScene()
    {
        sceneService.LoadScene(SceneType.RestfulScene);
    }
    public void ChangeEnergeticScene()
    {
        sceneService.LoadScene(SceneType.Energetic);
    }
}

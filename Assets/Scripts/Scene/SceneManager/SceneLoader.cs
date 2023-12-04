using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class SceneLoader : MonoBehaviour
{
    [Inject] private SceneService sceneService;

    public void LoadScene(int index)
    {
        sceneService.LoadScene(index);
    }
}

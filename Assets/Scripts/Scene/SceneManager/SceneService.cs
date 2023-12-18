using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneService : MonoBehaviour
{
    public delegate void SceneLoadedHandler(SceneType sceneType);
    public event SceneLoadedHandler OnSceneLoaded;


    public void LoadScene(SceneType sceneType)
    {
        StartCoroutine(LoadSceneAsync(sceneType));
    }

    public void LoadScene(int sceneIndex)
    {        StartCoroutine(LoadSceneAsync((SceneType)sceneIndex));
    }

    public void LoadSceneAdditive(SceneType sceneType)
    {
        StartCoroutine(LoadSceneAsyncAdditive(sceneType));
    }

    public void LoadSceneAdditive(int sceneIndex)
    {
        StartCoroutine(LoadSceneAsyncAdditive((SceneType)sceneIndex));
    }

    public void UnloadSceneAdditive(SceneType sceneType)
    {
        StartCoroutine(UnloadSceneAsyncAdditive(sceneType));
    }

    public void UnloadSceneAdditive(int sceneIndex)
    {
        StartCoroutine(UnloadSceneAsyncAdditive((SceneType)sceneIndex));
    }

  

    private IEnumerator LoadSceneAsync(SceneType sceneType)

    {
        var canvas = GameObject.Find("Canvas");

        GameObject loadingPrefab = Resources.Load<GameObject>("Prefab/Loading"); // Ustaw nazwê prefabu LoadingPrefab

        if (loadingPrefab != null)
        {
            GameObject loadingInstance = Instantiate(loadingPrefab, canvas.transform); // Tworzymy instancjê LoadingPrefab
        }
        AsyncOperation asyncOperation = SceneManager.LoadSceneAsync((int)sceneType);

        while (!asyncOperation.isDone)
        {
            yield return null;
        }

        OnSceneLoaded?.Invoke(sceneType);
    }

    private IEnumerator LoadSceneAsyncAdditive(SceneType sceneType)
    {
        AsyncOperation asyncOperation = SceneManager.LoadSceneAsync((int)sceneType, LoadSceneMode.Additive);

        while (!asyncOperation.isDone)
        {
            yield return null;
        }

        OnSceneLoaded?.Invoke(sceneType);
    }

    private IEnumerator UnloadSceneAsyncAdditive(SceneType sceneType)
    {
        AsyncOperation asyncOperation = SceneManager.UnloadSceneAsync((int)sceneType);

        while (!asyncOperation.isDone)
        {
            yield return null;
        }

        OnSceneLoaded?.Invoke(sceneType);
    }
}

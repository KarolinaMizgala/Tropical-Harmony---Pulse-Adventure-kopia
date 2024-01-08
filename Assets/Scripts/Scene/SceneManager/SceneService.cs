using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using Zenject;

/// <summary>
/// Service for managing scene transitions.
/// </summary>
public class SceneService : MonoBehaviour
{
    /// <summary>
    /// Delegate for handling scene loaded events.
    /// </summary>
    public delegate void SceneLoadedHandler(SceneType sceneType);

    /// <summary>
    /// Event triggered when a scene is loaded.
    /// </summary>
    public event SceneLoadedHandler OnSceneLoaded;
    private SceneType prevScene;
    [Inject] PlayerPositionManager playerPositionManager;
    /// <summary>
    /// Load a scene asynchronously.
    /// </summary>
    public void LoadScene(SceneType sceneType)
    {
        if (GetActivScene() == SceneType.RestfulScene || GetActivScene() == SceneType.Energetic)
        {
            playerPositionManager.SavePlayerPosition(GameObject.FindGameObjectWithTag("Player").transform.position);
        }
        if (sceneType == SceneType.RestfulScene || sceneType == SceneType.Energetic)
        {
            playerPositionManager.LoadPlayerPosition();
        }
        StartCoroutine(LoadSceneAsync(sceneType));
    }

    /// <summary>
    /// Load a scene asynchronously by scene index.
    /// </summary>
    public void LoadScene(int sceneIndex)
    {
        if (GetActivScene() == SceneType.RestfulScene || GetActivScene() == SceneType.Energetic)
        {
            playerPositionManager.SavePlayerPosition(GameObject.Find("Player").transform.position);
        }
        if (sceneIndex == (int)SceneType.RestfulScene || sceneIndex == (int)SceneType.Energetic)
        {
            playerPositionManager.LoadPlayerPosition();
        }
        StartCoroutine(LoadSceneAsync((SceneType)sceneIndex));
    }

    /// <summary>
    /// Load a scene additively and asynchronously.
    /// </summary>
    public void LoadSceneAdditive(SceneType sceneType)
    {
        StartCoroutine(LoadSceneAsyncAdditive(sceneType));
    }

    /// <summary>
    /// Load a scene additively and asynchronously by scene index.
    /// </summary>
    public void LoadSceneAdditive(int sceneIndex)
    {
        StartCoroutine(LoadSceneAsyncAdditive((SceneType)sceneIndex));
    }

    /// <summary>
    /// Unload a scene additively and asynchronously.
    /// </summary>
    public void UnloadSceneAdditive(SceneType sceneType)
    {
        StartCoroutine(UnloadSceneAsyncAdditive(sceneType));
    }

    /// <summary>
    /// Unload a scene additively and asynchronously by scene index.
    /// </summary>
    public void UnloadSceneAdditive(int sceneIndex)
    {
        StartCoroutine(UnloadSceneAsyncAdditive((SceneType)sceneIndex));
    }


    /// <summary>
    /// Coroutine to load a scene asynchronously.
    /// </summary>
    private IEnumerator LoadSceneAsync(SceneType sceneType)

    {
        var canvas = GameObject.Find("Canvas");

        GameObject loadingPrefab = Resources.Load<GameObject>("Prefab/Loading"); // Ustaw nazw� prefabu LoadingPrefab

        if (loadingPrefab != null)
        {
            GameObject loadingInstance = Instantiate(loadingPrefab, canvas.transform); // Tworzymy instancj� LoadingPrefab
        }
        AsyncOperation asyncOperation = SceneManager.LoadSceneAsync((int)sceneType);

        while (!asyncOperation.isDone)
        {
            yield return null;
        }

        OnSceneLoaded?.Invoke(sceneType);
    }
    /// <summary>
    /// Coroutine to load a scene additively and asynchronously.
    /// </summary>
    private IEnumerator LoadSceneAsyncAdditive(SceneType sceneType)
    {
        AsyncOperation asyncOperation = SceneManager.LoadSceneAsync((int)sceneType, LoadSceneMode.Additive);

        while (!asyncOperation.isDone)
        {
            yield return null;
        }

        OnSceneLoaded?.Invoke(sceneType);
    }

    /// <summary>
    /// Coroutine to unload a scene additively and asynchronously.
    /// </summary>
    private IEnumerator UnloadSceneAsyncAdditive(SceneType sceneType)
    {
        AsyncOperation asyncOperation = SceneManager.UnloadSceneAsync((int)sceneType);

        while (!asyncOperation.isDone)
        {
            yield return null;
        }

        OnSceneLoaded?.Invoke(sceneType);
    }

    public SceneType GetPrevScene()
    {
        return prevScene;
    }
    public void SetPrevScene(SceneType scene)
    {
        prevScene = scene;
    }
    public SceneType GetActivScene()
    {
        Scene scene = SceneManager.GetActiveScene();
        return (SceneType)scene.buildIndex;
    }
}

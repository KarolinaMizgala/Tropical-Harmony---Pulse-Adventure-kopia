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

    [Inject] 
    private PlayerPositionManager playerPositionManager; // Manages the player's position

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
    /// <summary>
    /// Gets the previous scene.
    /// </summary>
    /// <returns>The previous scene.</returns>
    public SceneType GetPrevScene()
    {
        return prevScene;
    }

    /// <summary>
    /// Sets the previous scene.
    /// </summary>
    /// <param name="scene">The scene to set as the previous scene.</param>
    public void SetPrevScene(SceneType scene)
    {
        prevScene = scene;
    }

    /// <summary>
    /// Gets the active scene.
    /// </summary>
    /// <returns>The active scene.</returns>
    public SceneType GetActivScene()
    {
        Scene scene = SceneManager.GetActiveScene();
        return (SceneType)scene.buildIndex;
    }
}

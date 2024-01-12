using UnityEngine;
using UnityEngine.SceneManagement;
using Zenject;

/// <summary>
/// Represents a pin in the game.
/// </summary>
public class Pin : MonoBehaviour
{
    /// <summary>
    /// The spawn point for the hit effect.
    /// </summary>
    [SerializeField]
    private Transform hitEffectSpawnPoint;

    /// <summary>
    /// The prefab for the hit effect.
    /// </summary>
    [SerializeField]
    private GameObject hitEffectPrefab;

    /// <summary>
    /// The dialog system.
    /// </summary>
    [Inject] DialogSystem dialogSystem;

    /// <summary>
    /// The level system.
    /// </summary>
    [Inject] LevelSystem levelSystem;

    /// <summary>
    /// The scene service.
    /// </summary>
    [Inject] SceneService sceneService;

    /// <summary>
    /// The 2D movement of the pin.
    /// </summary>
    private Movement2D movement2D;

    /// <summary>
    /// The game time.
    /// </summary>
    private float gameTime = 0f;

    /// <summary>
    /// Updates the game time every frame.
    /// </summary>
    private void Update()
    {
        gameTime += Time.deltaTime;
    }

    /// <summary>
    /// Initializes the pin.
    /// </summary>
    private void Awake()
    {
        movement2D = GetComponent<Movement2D>();
    }

    /// <summary>
    /// Handles the OnTriggerEnter2D event of the pin.
    /// </summary>
    /// <param name="collision">The collision data.</param>
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Target"))
        {
            movement2D.MoveTo(Vector3.zero);
            transform.SetParent(collision.transform);

            Destroy(this);
        }
        else if (collision.CompareTag("Pin"))
        {
           Debug.Log("GameOver");
            if (gameTime > 2f)
            {
                levelSystem.AddPoints(5);
                gameTime = 0;
            }
            dialogSystem.ShowConfirmationDialog("Do you want to try again?", Restart, Back);
        }
    }

    /// <summary>
    /// Goes back to the energetic scene.
    /// </summary>
    public void Back()
    {
        sceneService.LoadScene(SceneType.Energetic);
    }

    /// <summary>
    /// Restarts the current scene.
    /// </summary>
    public void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
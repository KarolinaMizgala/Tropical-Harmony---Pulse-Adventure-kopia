using System.Collections;
using UnityEngine;
using Zenject;

/// <summary>
/// Spawns obstacles in the game.
/// </summary>
public class SpawnObstacles : MonoBehaviour
{
    /// <summary>
    /// The obstacle prefab.
    /// </summary>
    [SerializeField] private GameObject obstaclePrefab;

    /// <summary>
    /// The maximum x-coordinate where an obstacle can spawn.
    /// </summary>
    [SerializeField] private float maxX;

    /// <summary>
    /// The minimum x-coordinate where an obstacle can spawn.
    /// </summary>
    [SerializeField] private float minX;

    /// <summary>
    /// The maximum y-coordinate where an obstacle can spawn.
    /// </summary>
    [SerializeField] private float maxY;

    /// <summary>
    /// The minimum y-coordinate where an obstacle can spawn.
    /// </summary>
    [SerializeField] private float minY;

    /// <summary>
    /// The time between each spawn.
    /// </summary>
    [SerializeField] private float timeBetweenSpawn;

    /// <summary>
    /// The tooltip GameObject.
    /// </summary>
    [SerializeField] private GameObject toolTip;

    /// <summary>
    /// The dialog system.
    /// </summary>
    [Inject] DialogSystem dialogSystem;

    /// <summary>
    /// Indicates whether the game has started.
    /// </summary>
    private bool gameStarted = false;

    /// <summary>
    /// The time when the next obstacle should spawn.
    /// </summary>
    private float timeSpawn;

    /// <summary>
    /// Waits until the tooltip becomes inactive and then starts the game.
    /// </summary>
    private IEnumerator Start()
    {
        // Wait until the tooltip becomes inactive
        while (toolTip.activeSelf)
        {
            yield return null;
        }

        // Start the game
        gameStarted = true;
    }

    /// <summary>
    /// Updates the game state every frame.
    /// </summary>
    void Update()
    {
        if (toolTip.activeSelf || dialogSystem.IsDialogVisible())
        {
            Time.timeScale = 0;
        }
        else
        {
            Time.timeScale = 1;
        }

        if (!gameStarted)
        {
            return;
        }

        if (Time.time > timeSpawn)
        {
            Spawn();
            timeSpawn = Time.time + timeBetweenSpawn;
        }
    }

    /// <summary>
    /// Spawns an obstacle at a random position.
    /// </summary>
    private void Spawn()
    {
        float randomX = Random.Range(minX, maxX);
        float randomY = Random.Range(minY, maxY);

        Instantiate(obstaclePrefab, transform.position + new Vector3(randomX, randomY, 0), transform.rotation);
    }
}
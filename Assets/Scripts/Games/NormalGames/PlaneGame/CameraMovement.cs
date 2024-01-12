using System.Collections;
using UnityEngine;
using Zenject;

/// <summary>
/// Controls the movement of the camera in the game.
/// </summary>
public class CameraMovement : MonoBehaviour
{
    /// <summary>
    /// The speed at which the camera moves.
    /// </summary>
    [SerializeField]
    private float cameraSpeed;

    /// <summary>
    /// The tooltip GameObject.
    /// </summary>
    [SerializeField]
    private GameObject toolTip;

    /// <summary>
    /// Indicates whether the game has started.
    /// </summary>
    private bool gameStarted = false;

    /// <summary>
    /// The dialog system.
    /// </summary>
    [Inject]
    DialogSystem dialogSystem;

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
    /// Updates the position of the camera every frame.
    /// </summary>
    void Update()
    {
        if (!gameStarted)
        {
            return;
        }

        if (toolTip.activeSelf || dialogSystem.IsDialogVisible())
        {
            Time.timeScale = 0;
        }
        else
        {
            Time.timeScale = 1;
        }

        transform.position += new Vector3(cameraSpeed * Time.deltaTime, 0, 0);
    }
}
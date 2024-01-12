using System.Collections;
using UnityEngine;
using Zenject;

/// <summary>
/// Spawns pins in the game.
/// </summary>
public class PinSpawner : MonoBehaviour
{
    /// <summary>
    /// The prefab for the pin.
    /// </summary>
    [SerializeField]
    private GameObject pinPrefab;

    /// <summary>
    /// The tooltip GameObject.
    /// </summary>
    [SerializeField]
    private GameObject toolTip;

    /// <summary>
    /// The dialog system.
    /// </summary>
    [Inject] DialogSystem dialogSystem;

    /// <summary>
    /// Waits until the tooltip becomes inactive.
    /// </summary>
    private IEnumerator Start()
    {
        while (toolTip.activeSelf)
        {
            yield return null;
        }
    }

    /// <summary>
    /// Updates the time scale every frame.
    /// </summary>
    private void Update()
    {
        if (toolTip.activeSelf || dialogSystem.IsDialogVisible())
        {
            Time.timeScale = 0;
        }
        else
        {
            Time.timeScale = 1;
        }
    }

    /// <summary>
    /// Spawns a pin at the position of the PinSpawner.
    /// </summary>
    public void OnClick()
    {
        Instantiate(pinPrefab, transform.position, Quaternion.identity);
    }
}
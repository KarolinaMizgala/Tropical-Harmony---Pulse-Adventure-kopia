using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Class representing a scene loading mechanism.
/// </summary>
public class SceneLoading : MonoBehaviour
{
    /// <summary>
    /// The speed of rotation in degrees per second.
    /// </summary>
    [SerializeField] private float rotationSpeed = 360f;

    /// <summary>
    /// The image to be rotated.
    /// </summary>
    [SerializeField] private Image image;

    /// <summary>
    /// Updates the rotation of the image every frame.
    /// </summary>
    void Update()
    {
        // Rotate the object by rotationSpeed degrees per second
        image.transform.Rotate(Vector3.forward, rotationSpeed * Time.deltaTime);
    }
}
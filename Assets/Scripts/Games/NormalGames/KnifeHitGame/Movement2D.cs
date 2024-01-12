using UnityEngine;

/// <summary>
/// Controls 2D movement for a GameObject.
/// </summary>
public class Movement2D : MonoBehaviour
{
    /// <summary>
    /// The speed at which the GameObject moves.
    /// </summary>
    [SerializeField]
    private float moveSpeed = 20;

    /// <summary>
    /// The direction in which the GameObject moves.
    /// </summary>
    [SerializeField]
    private Vector3 moveDirection = Vector3.up;

    /// <summary>
    /// Updates the position of the GameObject every frame.
    /// </summary>
    private void Update()
    {
        transform.position += moveDirection * moveSpeed * Time.deltaTime;
    }

    /// <summary>
    /// Changes the direction of movement for the GameObject.
    /// </summary>
    /// <param name="direction">The new direction for the GameObject to move in.</param>
    public void MoveTo(Vector3 direction)
    {
        moveDirection = direction;
    }
}
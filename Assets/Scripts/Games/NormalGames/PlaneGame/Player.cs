using UnityEngine;

/// <summary>
/// Represents the player in the game.
/// </summary>
public class Player : MonoBehaviour
{
    /// <summary>
    /// The speed at which the player moves.
    /// </summary>
    [SerializeField] private float playerSpeed;

    /// <summary>
    /// The Rigidbody2D component of the player.
    /// </summary>
    private Rigidbody2D rb;

    /// <summary>
    /// The direction in which the player is moving.
    /// </summary>
    private Vector2 playerDirection;

    /// <summary>
    /// Initializes the Rigidbody2D component.
    /// </summary>
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    /// <summary>
    /// Updates the player direction every frame.
    /// </summary>
    void Update()
    {
        float directionY = Input.GetAxisRaw("Vertical");
        playerDirection = new Vector2(0, directionY).normalized;
    }

    /// <summary>
    /// Updates the velocity of the Rigidbody2D component every fixed frame-rate frame.
    /// </summary>
    void FixedUpdate()
    {
        rb.velocity = new Vector2(0, playerDirection.y * playerSpeed);
    }
}
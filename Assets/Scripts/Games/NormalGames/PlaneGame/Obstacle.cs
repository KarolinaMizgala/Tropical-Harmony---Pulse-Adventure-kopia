using UnityEngine;

/// <summary>
/// Represents an obstacle in the game.
/// </summary>
public class Obstacle : MonoBehaviour
{
    /// <summary>
    /// The player GameObject.
    /// </summary>
    private GameObject player;

    /// <summary>
    /// Initializes the player GameObject.
    /// </summary>
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }

    /// <summary>
    /// Handles the collision of the obstacle with other GameObjects.
    /// </summary>
    /// <param name="collision">The Collider2D component of the GameObject that the obstacle collided with.</param>
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Border")
        {
            // Destroy the obstacle if it collides with the border
            Destroy(this.gameObject);
        }
        else if (collision.tag == "Player")
        {
            // Destroy the player if it collides with the obstacle
            Destroy(player.gameObject);
        }
    }
}
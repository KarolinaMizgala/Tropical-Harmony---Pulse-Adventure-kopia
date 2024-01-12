using System.Collections;
using UnityEngine;

/// <summary>
/// Class representing the player in the ColorJump game.
/// </summary>
public class ColorJumpPlayer : MonoBehaviour
{
    [SerializeField] private float jumpForce = 15f;
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private ColorJumpGameController gameController;

    private CircleCollider2D circleCollider2D;
    private Rigidbody2D rb2D;
    private SpriteRenderer spriteRenderer;

    /// <summary>
    /// Awake is called when the script instance is being loaded.
    /// </summary>
    private void Awake()
    {
        rb2D = GetComponent<Rigidbody2D>();
        circleCollider2D = GetComponent<CircleCollider2D>();
        rb2D.velocity = new Vector2(moveSpeed, jumpForce);
        spriteRenderer = GetComponent<SpriteRenderer>();
        StartCoroutine(nameof(UpdateInput));
    }

    /// <summary>
    /// Coroutine that waits for the player's input to jump.
    /// </summary>
    private IEnumerator UpdateInput()
    {
        while (true)
        {
            if (Input.GetMouseButtonDown(0))
            {
                JumpTo();
            }
            yield return null;
        }
    }

    /// <summary>
    /// Makes the player jump.
    /// </summary>
    private void JumpTo()
    {
        rb2D.velocity = new Vector2(rb2D.velocity.x, jumpForce);
    }

    /// <summary>
    /// Reverses the player's direction on the X axis.
    /// </summary>
    private void ReverseXDir()
    {
        float x = -Mathf.Sign(rb2D.velocity.x);
        rb2D.velocity = new Vector2(x * moveSpeed, rb2D.velocity.y);
    }

    /// <summary>
    /// Handles collision with a wall or a death wall.
    /// </summary>
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Wall"))
        {
            if (collision.GetComponent<SpriteRenderer>().color != spriteRenderer.color)
            {
                PlayerDie();
            }
            else
            {
                ReverseXDir();
                StartCoroutine(nameof(ColliderOnOfAnimation));
                gameController.CollisionWithWall();
            }
        }
        else if (collision.CompareTag("DeathWall"))
        {
            PlayerDie();
        }
    }

    /// <summary>
    /// Coroutine that disables and enables the player's collider.
    /// </summary>
    private IEnumerator ColliderOnOfAnimation()
    {
        circleCollider2D.enabled = false;
        yield return new WaitForSeconds(0.1f);
        circleCollider2D.enabled = true;
    }

    /// <summary>
    /// Handles the player's death.
    /// </summary>
    private void PlayerDie()
    {
        gameController.GameOver();
        gameObject.SetActive(false);
    }
}
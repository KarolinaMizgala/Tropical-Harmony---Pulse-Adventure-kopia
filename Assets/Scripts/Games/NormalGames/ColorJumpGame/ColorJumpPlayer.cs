using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject.Asteroids;
using Zenject.SpaceFighter;

public class ColorJumpPlayer : MonoBehaviour
{
    [SerializeField] private float jumpForce = 15f;
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private ColorJumpGameController gameController;

    private CircleCollider2D circleCollider2D;
    private Rigidbody2D rb2D;
    private SpriteRenderer spriteRenderer;

    private void Awake()
    {
        rb2D = GetComponent<Rigidbody2D>();
        circleCollider2D = GetComponent<CircleCollider2D>();
        rb2D.velocity = new Vector2(moveSpeed, jumpForce);
        spriteRenderer = GetComponent<SpriteRenderer>();
        StartCoroutine(nameof(UpdateInput));
    }
    private IEnumerator UpdateInput()
    {
        while (true)
        {
            if(Input.GetMouseButtonDown(0)) 
            {
                JumpTo();
            }
            yield return null;
        }
    }
    private void JumpTo()
    {
        rb2D.velocity =new Vector2(rb2D.velocity.x,jumpForce);
    }
    private void ReverseXDir()
    {
        float x = -Mathf.Sign(rb2D.velocity.x);
        rb2D.velocity = new Vector2(x * moveSpeed, rb2D.velocity.y);
    }
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
    private IEnumerator ColliderOnOfAnimation()
    {
        circleCollider2D.enabled = false;
        yield return new WaitForSeconds(0.1f);
        circleCollider2D.enabled = true;
    }
    private void PlayerDie()
    {
        gameController.GameOver();
       gameObject.SetActive(false);
    }
}

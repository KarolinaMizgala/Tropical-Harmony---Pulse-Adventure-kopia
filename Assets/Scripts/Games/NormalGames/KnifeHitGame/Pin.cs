using UnityEngine;
using UnityEngine.SceneManagement;
using Zenject;

public class Pin : MonoBehaviour
{
    [SerializeField]
    private Transform hitEffectSpawnPoint;
    [SerializeField]
    private GameObject hitEffectPrefab;

    [Inject] DialogSystem dialogSystem;
    [Inject] LevelSystem levelSystem;
    [Inject] SceneService sceneService;

    private Movement2D movement2D;

    private float gameTime = 0f;

    private void Update()
    {
        gameTime += Time.deltaTime;
    }
    private void Awake()
    {
        movement2D = GetComponent<Movement2D>();
    }

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
    public void Back()
    {
        sceneService.LoadScene(SceneType.Energetic);
    }
    public void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}

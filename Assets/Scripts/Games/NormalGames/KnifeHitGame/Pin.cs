using UnityEngine;
using UnityEngine.SceneManagement;

public class Pin : MonoBehaviour
{
	[SerializeField]
	private	Transform	hitEffectSpawnPoint;
	[SerializeField]
	private	GameObject	hitEffectPrefab;

	private	Movement2D	movement2D;

	private void Awake()
	{
		movement2D = GetComponent<Movement2D>();
	}

	private void OnTriggerEnter2D(Collider2D collision)
	{
		if ( collision.CompareTag("Target") )
		{
			movement2D.MoveTo(Vector3.zero);
			transform.SetParent(collision.transform);

			Instantiate(hitEffectPrefab, hitEffectSpawnPoint.position, hitEffectSpawnPoint.rotation);

			Camera.main.GetComponent<ShakeCamera>().Shake(0.1f, 1);

			Destroy(this);
		}
		else if ( collision.CompareTag("Pin") )
		{
			Debug.Log("GameOver");
			SceneManager.LoadScene(SceneManager.GetActiveScene().name);
		}
	}
}

using UnityEngine;

public class PinSpawner : MonoBehaviour
{
	[SerializeField]
	private	GameObject	pinPrefab;

	private void Update()
	{
		if ( Input.GetMouseButtonDown(0) )
		{
			Instantiate(pinPrefab, transform.position, Quaternion.identity);
		}
	}
}

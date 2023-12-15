using System.Collections;
using UnityEngine;

public class ShakeCamera : MonoBehaviour
{
	private	float	shakeTime;
	private	float	shakeIntensity;

	public void Shake(float shakeTime=1.0f, float shakeIntensity=0.1f)
	{
		this.shakeTime		= shakeTime;
		this.shakeIntensity	= shakeIntensity;

		StopCoroutine("OnShakeByRotation");
		StartCoroutine("OnShakeByRotation");
	}

	private IEnumerator OnShakeByRotation()
	{
		Vector3 start = transform.eulerAngles;

		while ( shakeTime > 0 )
		{
			float z = Random.Range(-1f, 1f);
			transform.rotation = Quaternion.Euler(start + new Vector3(0, 0, z) * shakeIntensity);

			shakeTime -= Time.deltaTime;

			yield return null;
		}

		transform.rotation = Quaternion.Euler(start);
	}
}

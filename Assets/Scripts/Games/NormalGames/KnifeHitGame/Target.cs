using System.Collections;
using UnityEngine;

/// <summary>
/// Represents a target in the game.
/// </summary>
public class Target : MonoBehaviour
{
    /// <summary>
    /// The speed at which the target rotates.
    /// </summary>
    private float rotateSpeed = 100;

    /// <summary>
    /// The angle at which the target rotates.
    /// </summary>
    private Vector3 rotateAngle = Vector3.forward;

    /// <summary>
    /// Starts the rotation of the target.
    /// </summary>
    private IEnumerator Start()
    {
        while (true)
        {
            int time = Random.Range(1, 5);

            yield return new WaitForSeconds(time);

            int speed = Random.Range(10, 300);
            int dir = Random.Range(0, 2);

            rotateSpeed = speed;
            rotateAngle = new Vector3(0, 0, dir * 2 - 1);
        }
    }

    /// <summary>
    /// Updates the rotation of the target every frame.
    /// </summary>
    private void Update()
    {
        transform.Rotate(rotateAngle * rotateSpeed * Time.deltaTime);
    }
}
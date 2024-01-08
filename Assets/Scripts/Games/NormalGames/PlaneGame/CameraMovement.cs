using System.Collections;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    [SerializeField] private float cameraSpeed;
    [SerializeField] private GameObject toolTip;
    private bool gameStarted = false;
    private IEnumerator Start()
    {

        // Czekaj, a¿ tooltip stanie siê nieaktywny
        while (toolTip.activeSelf)
        {
            yield return null;
        }


        // Rozpocznij grê
        gameStarted = true;

    }
    void Update()
    {
        if (!gameStarted)
        {
            return;
        }
        transform.position += new Vector3(cameraSpeed * Time.deltaTime, 0, 0);
    }
}

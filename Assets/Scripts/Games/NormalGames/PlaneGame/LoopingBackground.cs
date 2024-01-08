using System.Collections;
using UnityEngine;

public class LoopingBackground : MonoBehaviour
{
    [SerializeField] private float backgroundSpeed;
    [SerializeField] Renderer backgroundRenderer;
    [SerializeField] private GameObject toolTip;
    private bool gameStarted = false;
    private IEnumerator Start()
    {
        // Wyszukaj obiekt tooltip

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
        backgroundRenderer.material.mainTextureOffset += new Vector2(backgroundSpeed * Time.deltaTime, 0);
    }
}

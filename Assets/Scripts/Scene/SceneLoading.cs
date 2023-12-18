using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SceneLoading : MonoBehaviour
{
    [SerializeField] private float rotationSpeed = 360f; // szybkoœæ obrotu w stopniach na sekundê
    [SerializeField] private Image image;

    void Update()
    {
        // Obracamy obiekt o rotationSpeed stopni na sekundê
        image.transform.Rotate(Vector3.forward, rotationSpeed * Time.deltaTime);
    }
}

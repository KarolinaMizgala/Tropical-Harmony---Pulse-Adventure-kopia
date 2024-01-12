using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Controls the mouse look in the game.
/// </summary>
public class MouseLook : MonoBehaviour
{
    [SerializeField] 
    private float mouseSensitivity; // Mouse sensitivity
    public Transform player; // Player transform

    private float xRotation = 0f; // X rotation

    /// <summary>
    /// Update is called once per frame.
    /// </summary>
    void Update()
    {
        // Get the mouse X and Y input multiplied by the mouse sensitivity and the time delta
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

        // Subtract the mouseY from the xRotation
        xRotation -= mouseY;

        // Set the local rotation of the transform
        transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);

        // Rotate the player around the Y axis
        player.Rotate(Vector3.up * mouseX);
    }
}
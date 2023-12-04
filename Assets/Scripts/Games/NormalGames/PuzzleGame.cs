using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuzzleGame : MonoBehaviour
{
    [SerializeField] private Transform emptySpace;
    private Camera  _camera;
    private void Start()
    {
        _camera = Camera.main;
    }

    private void Update()
    {
        if(Input.GetMouseButtonDown(0))
            {
        Ray ray = _camera.ScreenPointToRay(Input.mousePosition);
            RaycastHit2D raycastHit2D = Physics2D.Raycast(ray.origin, ray.direction);
            if(raycastHit2D)
            {
                Debug.Log(raycastHit2D.transform.name);
                if(Vector2.Distance(emptySpace.position,raycastHit2D.transform.position)<2)
                {
                    Vector2 lastEmptySpacePosition = emptySpace.position;
                    emptySpace.position = raycastHit2D.transform.position;
                    raycastHit2D.transform.position = lastEmptySpacePosition;
                }
            }
        }
    }
}

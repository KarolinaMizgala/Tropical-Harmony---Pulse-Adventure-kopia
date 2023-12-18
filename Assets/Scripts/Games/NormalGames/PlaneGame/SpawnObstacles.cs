using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnObstacles : MonoBehaviour
{
    [SerializeField] private GameObject obstaclePrefab;
    [SerializeField] private float maxX;
    [SerializeField] private float minX;
    [SerializeField] private float maxY;
    [SerializeField] private float minY;
    [SerializeField] private float timeBetweenSpawn;

    private float timeSpawn;

    void Start()
    {
        if(Time.time > timeSpawn)
        {
            Spawn();
            timeSpawn = Time.time + timeBetweenSpawn;
           
        }
    }

    // Update is called once per frame
    private void Spawn()
    {
        float randomX = Random.Range(minX, maxX);
        float randomY = Random.Range(minY, maxY);

        Instantiate(obstaclePrefab, transform.position + new Vector3(randomX, randomY, 0), transform.rotation);
    }
}

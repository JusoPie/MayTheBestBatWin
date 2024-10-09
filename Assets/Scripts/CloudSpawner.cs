using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloudSpawner : MonoBehaviour
{
    public GameObject cloudPrefab; // Reference to the cloud prefab
    public float spawnInterval = 4f; // Time between spawns
    public float minY = -2f; // Minimum Y position for spawning clouds
    public float maxY = 2f; // Maximum Y position for spawning clouds

    private void Start()
    {
        // Start the coroutine for spawning clouds
        StartCoroutine(SpawnClouds());
    }

    private IEnumerator SpawnClouds()
    {
        while (true) // Infinite loop for continuous spawning
        {
            // Spawn a cloud at a random Y position
            float randomY = Random.Range(minY, maxY);
            Instantiate(cloudPrefab, new Vector3(10f, randomY, 0f), Quaternion.identity);

            // Wait for the specified interval before spawning the next cloud
            yield return new WaitForSeconds(spawnInterval);
        }
    }
}

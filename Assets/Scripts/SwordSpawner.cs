using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordSpawner : MonoBehaviour
{
    [SerializeField] private GameObject swordPrefab;  
    [SerializeField] private float spawnInterval = 2f; 
    [SerializeField] private float xOffset = 2f;      // How far off-screen the spawners are from the camera
    [SerializeField] private Vector2 spawnYRange = new Vector2(-3f, 3f);  // Random Y position range for swords reative to camera

    private Camera mainCamera;

    void Start()
    {
        mainCamera = Camera.main;  

        
        StartCoroutine(SpawnSwords());
    }

    IEnumerator SpawnSwords()
    {
        while (true)
        {
            
            yield return new WaitForSeconds(spawnInterval);

            // Randomly choose whether to spawn from the left or right
            bool spawnFromLeft = Random.Range(0, 2) == 0;

            
            float randomY = Random.Range(spawnYRange.x, spawnYRange.y) + mainCamera.transform.position.y;

            // Calculate spawner positions just outside the camera's view based on the camera's current position
            float cameraWidth = mainCamera.orthographicSize * mainCamera.aspect;
            Vector3 leftSpawnerPosition = new Vector3(mainCamera.transform.position.x - cameraWidth - xOffset, randomY, 0);
            Vector3 rightSpawnerPosition = new Vector3(mainCamera.transform.position.x + cameraWidth + xOffset, randomY, 0);

            // Instantiate the sword at the chosen spawner position
            Vector3 spawnPosition = spawnFromLeft ? leftSpawnerPosition : rightSpawnerPosition;

            // Instantiate sword
            GameObject sword = Instantiate(swordPrefab, spawnPosition, Quaternion.identity);

            // Set the direction of the sword based on where it was spawned (left or right)
            OneSword swordMovement = sword.GetComponent<OneSword>();
            if (swordMovement != null)
            {
                swordMovement.speed = spawnFromLeft ? Mathf.Abs(swordMovement.speed) : -Mathf.Abs(swordMovement.speed);
            }
        }
    }
}

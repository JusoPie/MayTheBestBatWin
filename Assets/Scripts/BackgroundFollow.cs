using UnityEngine;

public class BackgroundDrop : MonoBehaviour
{
    public Camera mainCamera;
    public float dropSpeed = 1.0f; // Speed of dropping down

    void Update()
    {
        // Get the camera's position
        Vector3 cameraPosition = mainCamera.transform.position;

        // Update the position to follow the camera's X position and drop down
        Vector3 newPosition = new Vector3(cameraPosition.x, transform.position.y - dropSpeed * Time.deltaTime, transform.position.z);

        // Apply the new position
        transform.position = newPosition;
    }
}
using UnityEngine;

public class BackgroundFollow : MonoBehaviour
{
    public Camera mainCamera;

    void Start()
    {
        if (mainCamera == null)
        {
            mainCamera = Camera.main; // Get the main camera if not assigned
        }
    }

    void Update()
    {
        // Follow the camera's position
        Vector3 cameraPosition = mainCamera.transform.position;
        transform.position = new Vector3(cameraPosition.x, cameraPosition.y, transform.position.z);


    }
}

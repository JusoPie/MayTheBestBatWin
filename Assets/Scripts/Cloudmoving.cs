using UnityEngine;

public class CloudMover : MonoBehaviour
{
    public float speed = 1f;  // Speed of the cloud movement
    public float resetPositionX = -10f; // Where to reset the cloud position
    public float startPositionX = 10f; // Starting position of the cloud

    private void Start()
    {
        // Set the starting position
        transform.position = new Vector3(startPositionX, transform.position.y, transform.position.z);
    }

    private void Update()
    {
        // Move the cloud
        transform.position += Vector3.left * speed * Time.deltaTime;

        // Reset position if it moves off-screen
        if (transform.position.x < resetPositionX)
        {
            transform.position = new Vector3(startPositionX, transform.position.y, transform.position.z);
        }
    }
}
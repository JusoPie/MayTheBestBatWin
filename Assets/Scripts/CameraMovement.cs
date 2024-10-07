using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 2f;  // Speed at which the camera moves upwards

    void Update()
    {
        // Move the camera upwards at a constant speed over time
        transform.position += new Vector3(0, moveSpeed * Time.deltaTime, 0);
    }
}


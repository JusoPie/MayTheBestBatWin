using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomPlatform : MonoBehaviour
{
    private BoxCollider2D platformCollider;
    private Vector3 initialPosition; // To reset the platform position after teleporting
    public float teleportDistance = 2f;  // Distance the platform will teleport
    [SerializeField] private float rotationSpeed = 90f; // Speed of rotation in degrees per second
    private bool isRotating = false;

    void Start()
    {
        // Get the BoxCollider2D component attached to the platform
        platformCollider = GetComponent<BoxCollider2D>();
        // Store the platform's initial position
        initialPosition = transform.position;
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            // Randomly select an action: 0 = Disable collider, 1 = Teleport Left, 2 = Teleport Right,
            // 3 = Teleport Up, 4 = Teleport Down, 5 = Rotate on Z-axis, 6 = Do nothing
            int randomAction = Random.Range(0, 7);

            switch (randomAction)
            {
                case 0:
                    // Disable the platform's collider
                    DisableCollider();
                    break;
                case 1:
                    // Teleport platform left
                    TeleportPlatform(Vector2.left);
                    break;
                case 2:
                    // Teleport platform right
                    TeleportPlatform(Vector2.right);
                    break;
                case 3:
                    // Teleport platform up
                    TeleportPlatform(Vector2.up);
                    break;
                case 4:
                    // Teleport platform down
                    TeleportPlatform(Vector2.down);
                    break;
                case 5:
                    // Rotate platform on Z-axis
                    StartRotating();
                    break;
                case 6:
                    // Do nothing
                    Debug.Log("No action taken");
                    break;
            }
        }
    }

    private void DisableCollider()
    {
        platformCollider.enabled = false;
        Debug.Log("Platform collider disabled");

        // enable the collider after the coroutine
        StartCoroutine(ReenableCollider());
    }

    private IEnumerator ReenableCollider()
    {
        yield return new WaitForSeconds(2f);
        platformCollider.enabled = true;
    }

    private void TeleportPlatform(Vector2 direction)
    {
        // Teleport the platform by changing its position in the specified direction
        Vector3 teleportPosition = initialPosition + (Vector3)direction * teleportDistance;
        transform.position = teleportPosition;
    }

    private void StartRotating()
    {
        if (!isRotating)
        {
            StartCoroutine(RotatePlatform());
        }
    }

    private IEnumerator RotatePlatform()
    {
        isRotating = true;
        float rotationTime = 2f;  
        float elapsedTime = 0f;

        while (elapsedTime < rotationTime)
        {
            // Rotate the platform around the Z-axis
            transform.Rotate(Vector3.forward * rotationSpeed * Time.deltaTime);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        isRotating = false;
    }
}


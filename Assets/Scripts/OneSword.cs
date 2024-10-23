using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OneSword : MonoBehaviour
{
    public float speed = 5f;
    [SerializeField] private int damage = 20;

    void Update()
    {
        // Move the sword across the x-axis by speed every frame
        transform.Translate(Vector3.right * speed * Time.deltaTime);

        // Get the camera bounds for off-screen detection
        Vector3 screenBoundsLeft = Camera.main.ViewportToWorldPoint(new Vector3(0, 0, 0));  // Left edge of screen
        Vector3 screenBoundsRight = Camera.main.ViewportToWorldPoint(new Vector3(1, 0, 0)); // Right edge of screen

        // Destroy sword if it goes off-screen on the left or right, depending on its direction
        if (transform.position.x < screenBoundsLeft.x - 1 || transform.position.x > screenBoundsRight.x + 1)
        {
            Destroy(gameObject);
        }
    }

    // Handle collision with player
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Health playerHealth = collision.gameObject.GetComponent<Health>();

            if (playerHealth != null)
            {
                playerHealth.TakeDamage(damage, transform.position); // Apply damage
            }

            Destroy(gameObject); // Optionally destroy sword upon hitting player
        }
    }
}

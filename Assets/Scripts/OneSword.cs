using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OneSword : MonoBehaviour
{
    public float speed = 5f;  // Speed of the sword's movement on the x-axis
    [SerializeField] private int damage = 20;   // Amount of damage the sword deals

    // Update is called once per frame
    void Update()
    {
        // Move the sword across the x-axis by speed every frame
        transform.Translate(Vector3.right * speed * Time.deltaTime);

        if (transform.position.x > Camera.main.ViewportToWorldPoint(new Vector3(1, 0, 0)).x + 1)
        {
            Destroy(gameObject); // Destroy the sword once it moves off the right side of the screen
        }
    }

    // Trigger detection for hitting the player
    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Check if the object hit is tagged as "Player"
        if (collision.gameObject.CompareTag("Player"))
        {
            // Attempt to get the Health component from the Player
            Health playerHealth = collision.gameObject.GetComponent<Health>();

            // If the Player has a Health component, apply damage
            if (playerHealth != null)
            {
                playerHealth.TakeDamage(damage, transform.position);
            }
        }
    }
}


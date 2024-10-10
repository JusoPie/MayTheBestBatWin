using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BatPowder : MonoBehaviour
{
    [SerializeField] private int damagePerSecond = 5;
    private void OnTriggerStay2D(Collider2D other)
    {
        // Check if the object inside the collider is the player
        if (other.CompareTag("Player"))
        {
            // Get the player's Health component
            Health playerHealth = other.GetComponent<Health>();
            if (playerHealth != null)
            {
                // Deal damage over time
                playerHealth.TakeDamage(damagePerSecond, transform.position);
            }
        }
    }
}

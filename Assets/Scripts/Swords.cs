using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Swords : MonoBehaviour
{
    [SerializeField] private int damage = 20;
    [SerializeField] private Vector3 defaultPosition; // The position to which the swords should move after 5 seconds
    [SerializeField] private float riseDuration = 1f; // Time it takes for the swords to rise
    [SerializeField] private float delayBeforeRising = 5f; // Delay before the swords start rising

    private Vector3 offScreenPosition; // The starting position below the camera

    void Start()
    {
        // Set the off-screen position, below the camera's current position
        offScreenPosition = new Vector3(defaultPosition.x, Camera.main.transform.position.y - 10f, defaultPosition.z);

        // Move swords to the off-screen position at the start
        transform.position = offScreenPosition;

        // Start the coroutine to move swords after the delay
        StartCoroutine(RiseSwordsAfterDelay());
    }

    IEnumerator RiseSwordsAfterDelay()
    {
        // Wait for the delay before rising
        yield return new WaitForSeconds(delayBeforeRising);

        // Gradually move the swords from the off-screen position to the default position
        float elapsedTime = 0f;

        while (elapsedTime < riseDuration)
        {
            transform.position = Vector3.Lerp(offScreenPosition, defaultPosition, elapsedTime / riseDuration);
            elapsedTime += Time.deltaTime;
            yield return null; // Wait for the next frame
        }

        // Ensure the swords end up at the exact default position
        transform.position = defaultPosition;
    }

    // Collision detection with the player
    public void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Health enemyHealth = collision.gameObject.GetComponent<Health>();
            if (enemyHealth != null)
            {
                enemyHealth.TakeDamage(damage, transform.position);
            }
        }
    }
}


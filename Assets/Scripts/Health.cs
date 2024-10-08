using System.Collections;
using UnityEngine;

public class Health : MonoBehaviour
{
    public int maxHealth = 100;
    [SerializeField] private int currentHealth;
    private Animator animator;

    [SerializeField] private float respawnDelay = 3f; // Delay before respawn
    [SerializeField] private float pushBackForce = 5f; // Amount of force applied when hit
    [SerializeField] private float pushbackEffect = 1f; //Time to wait before enabling canMove after damage

    private Vector3 initialPosition; // Store the initial position for respawn
    private SpriteRenderer spriteRenderer; // Reference to the SpriteRenderer for transparency
    private PlayerMovement playerMovement; // Reference to PlayerMovement to disable movement
    private Rigidbody2D rb; // Reference to Rigidbody2D for applying pushback
    private Color originalColor; // Store original sprite color

    private bool canTakeDamage = true;

    private void Start()
    {
        animator = GetComponent<Animator>();
        currentHealth = maxHealth;

        spriteRenderer = GetComponent<SpriteRenderer>(); // Get the SpriteRenderer component
        playerMovement = GetComponent<PlayerMovement>(); // Get the PlayerMovement component
        rb = GetComponent<Rigidbody2D>(); // Get the Rigidbody2D component

        originalColor = spriteRenderer.color; // Save the original color of the sprite
        initialPosition = transform.position; // Save the initial position
    }

    public void TakeDamage(int damage, Vector2 damageSourcePosition)
    {
        if (canTakeDamage) 
        {
            Debug.Log("damage");
            currentHealth -= damage;
            playerMovement.enabled = false;
            animator.SetTrigger("Damage");
            StartCoroutine(KnockbackTimer());

            // Apply pushback force
            ApplyPushback(damageSourcePosition);

            if (currentHealth <= 0)
            {
                Debug.Log("kuolin");
                Die();
            }
        }
        

    }

    private void ApplyPushback(Vector2 damageSourcePosition)
    {
        // Calculate direction of the pushback (from the source of the damage)
        Vector2 pushDirection = (transform.position - new Vector3(damageSourcePosition.x, damageSourcePosition.y)).normalized;

        // Apply a force in the pushback direction with the specified pushback force
        rb.AddForce(pushDirection * pushBackForce, ForceMode2D.Impulse);
    }

    private void Die()
    {
        // Disable player movement
        playerMovement.enabled = false;
        playerMovement.isDead = true;
        canTakeDamage = false;

        // Set the sprite to transparent to simulate disappearance
        SetSpriteTransparency(0.2f);



        // Start respawn process after a delay
        StartCoroutine(RespawnAfterDelay());

    }

    private IEnumerator KnockbackTimer() 
    {
        yield return new WaitForSeconds(pushbackEffect);

        playerMovement.enabled = true;
    }

    private IEnumerator RespawnAfterDelay()
    {
        yield return new WaitForSeconds(respawnDelay);

        // Reset health and position
        currentHealth = maxHealth;
        transform.position = initialPosition; // Optional: Reset to initial position

        // Restore the sprite's visibility
        SetSpriteTransparency(1f);

        // Re-enable player movement
        playerMovement.enabled = true;
        playerMovement.isDead = false;
        Debug.Log("PM enabled");
        canTakeDamage = true;


        rb.velocity = Vector2.zero;
    }

    private void SetSpriteTransparency(float alpha)
    {
        // Change the sprite's transparency (alpha value)
        Color newColor = spriteRenderer.color;
        newColor.a = alpha;
        spriteRenderer.color = newColor;
    }
}

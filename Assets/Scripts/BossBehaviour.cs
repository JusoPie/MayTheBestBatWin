using UnityEngine;
using System.Collections;

public class BossBehavior : MonoBehaviour
{
    public float moveSpeed = 2f;        // Speed of the boss movement
    public float attackRange = 2f;      // Distance at which boss will attack
    public float raycastRange = 5f;     // Distance the raycast checks for the player
    public float attackCooldown = 2f;   // Time between attacks

    private Transform player;
    private Rigidbody2D rb;
    private Animator animator;
    private bool isAttacking = false;
    private bool canAttack = true;

    private Vector3 originalScale;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        player = GameObject.FindGameObjectWithTag("Player").transform; 
    }

    void Update()
    {
        if (player != null && !isAttacking)
        {
            MoveTowardsPlayer();

            // Check for attack using raycast
            RaycastHit2D hit = Physics2D.Raycast(transform.position, GetDirectionToPlayer(), raycastRange);
            if (hit.collider != null && hit.collider.CompareTag("Player") && Vector2.Distance(transform.position, player.position) <= attackRange)
            {
                if (canAttack)
                {
                    StartCoroutine(Attack());
                }
            }
        }
    }

    // Move towards the player
    void MoveTowardsPlayer()
    {
        Vector2 direction = GetDirectionToPlayer();
        rb.velocity = new Vector2(direction.x * moveSpeed, rb.velocity.y);

        // Flip the sprite depending on player's position
        if (direction.x > 0)
            transform.localScale = new Vector3(originalScale.x, originalScale.y, originalScale.z); // Facing right
        else if (direction.x < 0)
            transform.localScale = new Vector3(-originalScale.x, originalScale.y, originalScale.z); // Facing left
    }

    // Get direction towards the player
    Vector2 GetDirectionToPlayer()
    {
        return (player.position - transform.position).normalized;
    }

    // Attack the player
    IEnumerator Attack()
    {
        isAttacking = true;
        canAttack = false;

        // Trigger attack animation
        animator.SetTrigger("Attack");

        // You can add logic here for dealing damage if in range

        // Wait for the attack animation to complete (you may need to adjust this based on your animation length)
        yield return new WaitForSeconds(0.5f);

        isAttacking = false;

        // Cooldown between attacks
        yield return new WaitForSeconds(attackCooldown);
        canAttack = true;
    }

    // Debugging the raycast in the Scene view
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position, transform.position + (Vector3)GetDirectionToPlayer() * raycastRange);
    }
}

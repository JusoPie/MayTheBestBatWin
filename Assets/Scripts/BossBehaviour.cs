using UnityEngine;
using System.Collections;

public class BossBehavior : MonoBehaviour
{
    [SerializeField] private Transform player1;
    [SerializeField] private Transform player2;
    [SerializeField] private float speed = 2f;
    [SerializeField] private float attackRange = 1.5f;  
    [SerializeField] private float secondAttackSpeed = 5f; 
    [SerializeField] private int health = 100;   

    private SpriteRenderer spriteRenderer;  
    private Transform targetPlayer;  // Current target player
    private Animator animator;
    private bool isAttacking = false;  
    private Vector3 initialScale;  // Store the initial scale of the boss

    private void Start()
    {
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        targetPlayer = GetClosestPlayer();  // Initialize with closest player
        
        // Store the initial scale at the start
        initialScale = transform.localScale;

        animator.SetTrigger("Prepare");
    }

    private void Update()
    {
        if (health <= 0)
        {
            DestroyBoss();
            return;
        }

        // Move towards the closest player if not attacking
        if (!isAttacking)
        {
            MoveTowardsTarget();
        }

        // Check target player
        Transform closestPlayer = GetClosestPlayer();
        if (targetPlayer != closestPlayer)
        {
            targetPlayer = closestPlayer;
        }

        // Check for attack range (using both X and Y distance)
        float distanceToTarget = Vector2.Distance(transform.position, targetPlayer.position);
        if (distanceToTarget <= attackRange)
        {
            Attack();
        }
    }

    // Get the closest player between player1 and player2
    private Transform GetClosestPlayer()
    {
        float distanceToPlayer1 = Vector2.Distance(transform.position, player1.position);
        float distanceToPlayer2 = Vector2.Distance(transform.position, player2.position);

        return distanceToPlayer1 < distanceToPlayer2 ? player1 : player2;
    }

    // Move the boss towards the target player
    private void MoveTowardsTarget()
    {
        // Flip only the X-axis based on the target player's position
        if (targetPlayer.position.x > transform.position.x)
        {
            transform.localScale = new Vector3(initialScale.x, initialScale.y, initialScale.z);  // Face right
        }
        else
        {
            transform.localScale = new Vector3(-initialScale.x, initialScale.y, initialScale.z); // Face left (flip X)
        }

        // Move towards the target player in both X and Y axes
        transform.position = Vector2.MoveTowards(transform.position, targetPlayer.position, speed * Time.deltaTime);
    }

    private void Attack()
    {
        if (isAttacking) return;

        isAttacking = true;
        animator.SetTrigger("Attack1");  
        Invoke("PerformSecondAttack", 1f);  
    }

    // Perform second attack
    private void PerformSecondAttack()
    {
        // If still in range after the first attack, perform the second attack
        float distanceToTarget = Vector2.Distance(transform.position, targetPlayer.position);
        if (distanceToTarget <= attackRange)
        {
            animator.SetTrigger("Attack2");  
            // Quickly move towards the player during the second attack
            transform.position = Vector2.MoveTowards(transform.position, targetPlayer.position, secondAttackSpeed * Time.deltaTime);
        }

        Invoke("EndAttack", 1f);  
    }

    // End attack logic
    private void EndAttack()
    {
        isAttacking = false;
    }

    
    public void TakeDamage(int damage)
    {
        health -= damage;

        if (health > 0)
        {
            StartCoroutine(FlashRed());
        }
        else
        {
            DestroyBoss();
        }
    }

    // Flash red when damaged
    private IEnumerator FlashRed()
    {
        spriteRenderer.color = Color.red;
        yield return new WaitForSeconds(0.1f);  
        spriteRenderer.color = Color.white;  
    }

   
    private void DestroyBoss()
    {
        animator.SetTrigger("Die");
        Destroy(gameObject);
    }
}

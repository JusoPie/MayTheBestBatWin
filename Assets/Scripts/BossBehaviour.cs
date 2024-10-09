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
    private Vector3 initialScale;

    private void Start()
    {
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        targetPlayer = GetClosestPlayer();  // Initialize with closest player
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
            animator.SetBool("isWalking", true);
            MoveTowardsTarget();
        }
        else 
        {
            animator.SetBool("isWalking", false);
        }

        // Check target player
        Transform closestPlayer = GetClosestPlayer();
        if (targetPlayer != closestPlayer)
        {
            targetPlayer = closestPlayer;
        }

        // Check for attack range
        float distanceToTarget = Mathf.Abs(transform.position.x - targetPlayer.position.x);
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
        
        if (targetPlayer.position.x > transform.position.x)
        {
            transform.localScale = new Vector3(initialScale.x, initialScale.y, initialScale.z);  
        }
        else
        {
            transform.localScale = new Vector3(-initialScale.x, initialScale.y, initialScale.z); 
        }

        // Move towards the target player
        transform.position = Vector2.MoveTowards(transform.position, new Vector2(targetPlayer.position.x, transform.position.y), speed * Time.deltaTime);
        
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
        float distanceToTarget = Mathf.Abs(transform.position.x - targetPlayer.position.x);
        if (distanceToTarget <= attackRange)
        {
            animator.SetTrigger("Attack2");  
            // Quickly move towards the player during the second attack
            transform.position = Vector2.MoveTowards(transform.position, new Vector2(targetPlayer.position.x, transform.position.y), secondAttackSpeed * Time.deltaTime);
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

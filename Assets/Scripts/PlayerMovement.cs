using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private Rigidbody2D _rb;
    [SerializeField] private Transform _groundCheck;
    [SerializeField] private LayerMask _groundLayer;
    [SerializeField] private GameObject transformEffect;

    private float horizontal;
    [SerializeField] private float speed = 8f;
    [SerializeField] private float jumpingPower = 16f;
    [SerializeField] private bool isFacingright = true;
    private Animator animator;
    private PlayerInput playerInput;

    private bool wasGrounded;
    private bool canMove = true; // Control when the player can move

    [SerializeField] private float attackCooldown = 0.5f; // Adjustable attack cooldown
    [SerializeField] private float attackRange = 1f; // Range of the attack (raycast length)
    [SerializeField] private int attackDamage = 10; // Damage dealt by attack
    [SerializeField] private LayerMask enemyLayer; // Layer mask for enemies

    private bool canAttack = true; // Track if the player can attack
    public bool isDead = false;

    private void Start()
    {
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (canMove) // Only allow movement if canMove is true
        {
            _rb.velocity = new Vector2(horizontal * speed, _rb.velocity.y);
        }

        if (!isFacingright && horizontal > 0f)
        {
            Flip();
        }
        else if (isFacingright && horizontal < 0f)
        {
            Flip();
        }

        if (horizontal != 0)
        {
            animator.SetBool("IsWalking", true);
        }
        else
        {
            animator.SetBool("IsWalking", false);
        }

        bool isGrounded = IsGrounded();

        // Instantiate particle effect when the player lands on the ground
        if (isGrounded && !wasGrounded)
        {
            Instantiate(transformEffect, _groundCheck.position, Quaternion.identity);
        }

        // Update animator's grounded state
        animator.SetBool("isGrounded", isGrounded);

        // Update wasGrounded
        wasGrounded = isGrounded;
    }

    private void OnEnable()
    {
        playerInput = GetComponent<PlayerInput>();
        playerInput.actions.Enable();  // Enable input actions
    }

    private void OnDisable()
    {
        playerInput = GetComponent<PlayerInput>();
        playerInput.actions.Disable(); // Disable input actions
    }

    public void Jump(InputAction.CallbackContext context)
    {
        if (context.performed && canMove)
        {
            _rb.velocity = new Vector2(_rb.velocity.x, jumpingPower);
            animator.SetTrigger("Jump");
            SoundManager.instance.PlaySFX(1);
        }

        if (context.canceled && _rb.velocity.y > 0f && canMove)
        {
            _rb.velocity = new Vector2(_rb.velocity.x, _rb.velocity.y * 0.5f);
        }

        if (IsGrounded())
        {
            Instantiate(transformEffect, _groundCheck.position, Quaternion.identity);
        }
    }

    public void Attack(InputAction.CallbackContext context)
    {
        if (context.performed && canAttack && !isDead) // Only attack if cooldown has passed or is not dead
        {
            canAttack = false; // Prevent further attacks until cooldown finishes
            canMove = false;

            _rb.bodyType = RigidbodyType2D.Kinematic;
            _rb.velocity = Vector2.zero;
            animator.SetTrigger("Attack");
            SoundManager.instance.PlaySFX(7);

            // Cast raycast in the direction the player is facing
            RaycastHit2D hit = Physics2D.Raycast(transform.position, isFacingright ? Vector2.right : Vector2.left, attackRange, enemyLayer);

            if (hit.collider != null)
            {
                // Apply damage to the hit object if it has a health component
                Health enemyHealth = hit.collider.GetComponent<Health>();
                if (enemyHealth != null)
                {
                    enemyHealth.TakeDamage(attackDamage, transform.position);
                }

                // Check if the hit object has a BossBehaviour component (for boss)
                BossBehavior boss = hit.collider.GetComponent<BossBehavior>();
                if (boss != null)
                {
                    boss.TakeDamage(attackDamage);
                }
            }

            

            // Start cooldown coroutine
            StartCoroutine(AttackCooldown());
        }
    }

    private IEnumerator AttackCooldown()
    {
        // Wait for the attack to complete and cooldown to finish
        yield return new WaitForSeconds(0.1f); // Wait for the attack animation or effect to finish
        ResumeMovement(); // Resume movement
        yield return new WaitForSeconds(attackCooldown); // Wait for the cooldown time
        canAttack = true; // Allow attacking again
    }

    private void ResumeMovement()
    {
        _rb.bodyType = RigidbodyType2D.Dynamic;
        canMove = true;
    }

    private bool IsGrounded()
    {
        return Physics2D.OverlapCircle(_groundCheck.position, 0.2f, _groundLayer);
    }



    private void Flip()
    {
        isFacingright = !isFacingright;
        Vector3 localScale = transform.localScale;
        localScale.x *= -1f;
        transform.localScale = localScale;
    }

    public void Move(InputAction.CallbackContext context)
    {
        if (canMove)
        {
            horizontal = context.ReadValue<Vector2>().x;
        }
        else
        {
            horizontal = 0f;
        }
    }
}

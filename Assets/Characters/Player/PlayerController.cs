using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 1f;
    public float collisionOffset = 0.05f;
    public ContactFilter2D movementFilter;
    public SwordAttack swordAttack;

    Vector2 movementInput;
    SpriteRenderer spriteRenderer;
    Rigidbody2D rb;
    Animator animator;
    List<RaycastHit2D> castCollisions = new List<RaycastHit2D>();

    bool canMove = true;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void FixedUpdate()
    {
        if (canMove)
        {
            if (movementInput != Vector2.zero)
            {
                bool success = TryMove(movementInput);

                if (!success)
                {
                    success = TryMove(new Vector2(movementInput.x, 0));
                }
                if (!success)
                {
                    success = TryMove(new Vector2(0, movementInput.y));
                }

                animator.SetBool("isMoving", success);
            }
            else
            {
                animator.SetBool("isMoving", false);
            }

            // Flip sprite based on movement direction
            if (movementInput.x < 0)
            {
                spriteRenderer.flipX = true;
            }
            else if (movementInput.x > 0)
            {
                spriteRenderer.flipX = false;
            }
        }
    }

    private bool TryMove(Vector2 direction)
    {
        if (direction != Vector2.zero)
        {
            int count = rb.Cast(
                    direction,
                    movementFilter,
                    castCollisions,
                    moveSpeed * Time.fixedDeltaTime + collisionOffset);

            if (count == 0)
            {
                rb.MovePosition(rb.position + direction * moveSpeed * Time.fixedDeltaTime);
                return true;
            }
            else
            {
                return false;
            }
        }
        else
        {
            return false;
        }
    }

    void OnMove(InputValue movementValue)
    {
        movementInput = movementValue.Get<Vector2>();
    }

    void Update()
    {
        // Check if left mouse button is pressed for chest interaction or attacking
        if (Mouse.current.leftButton.wasPressedThisFrame)
        {
            // First, check if we're clicking on a chest
            HandleChestInteraction();

            // If not interacting with chest, perform an attack
            if (!InteractingWithChest())
            {
                OnAttack();  // Attack (e.g., destroy slimes)
            }
        }
    }

    void HandleChestInteraction()
    {
        Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue());
        RaycastHit2D hit = Physics2D.Raycast(mousePosition, Vector2.zero);

        if (hit.collider != null)
        {
            ChestController chest = hit.collider.GetComponent<ChestController>();
            if (chest != null)
            {
                // Open or close the chest based on its current state and distance to player
                if (!chest.isOpen && Vector2.Distance(transform.position, chest.transform.position) <= 0.75f)
                {
                    chest.OpenChest();  // Now accessible
                }
                else if (chest.isOpen && Vector2.Distance(transform.position, chest.transform.position) > 0.75f)
                {
                    chest.CloseChest();  // Close chest if moving away
                }
            }
        }
    }

    // Method to check if we are interacting with a chest
    bool InteractingWithChest()
    {
        Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue());
        RaycastHit2D hit = Physics2D.Raycast(mousePosition, Vector2.zero);

        if (hit.collider != null)
        {
            ChestController chest = hit.collider.GetComponent<ChestController>();
            return chest != null;  // If we're hitting a chest, we're interacting with it
        }

        return false;  // Not interacting with a chest
    }

    void OnAttack()
    {
        animator.SetTrigger("swordAttack");
        SwordAttack();
    }

    public void SwordAttack()
    {
        LockMovement();
        if (spriteRenderer.flipX == true)
        {
            swordAttack.AttackLeft();
        }
        else
        {
            swordAttack.AttackRight();
        }
    }

    public void EndSwordAttack()
    {
        UnlockMovement();
        swordAttack.StopAttack();
    }

    public void LockMovement()
    {
        canMove = false;
    }

    public void UnlockMovement()
    {
        canMove = true;
    }
}

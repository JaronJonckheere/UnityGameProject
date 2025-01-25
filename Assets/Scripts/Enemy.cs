using UnityEngine;
using System.Collections;  // Add this line to use IEnumerator

public class Enemy : MonoBehaviour
{
    public float health = 10f;
    public Animator animator;  // Reference to the enemy's Animator component
    private bool isDead = false;  // To track if the enemy is already dead

    private void Start()
    {
        animator = GetComponent<Animator>();  // Make sure the enemy has an Animator component
    }

    public void TakeDamage(float damage)
    {
        if (isDead) return;  // If already dead, do nothing

        health -= damage;

        // Trigger the damage animation
        animator.SetTrigger("Damage");

        if (health <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        if (isDead) return;  // Prevent multiple calls to Die
        isDead = true;

        // Play the death animation
        animator.SetTrigger("Defeated");

        // Disable the enemy's collider so it no longer interacts with the player
        GetComponent<Collider2D>().enabled = false;

        // Optionally, disable enemy movement or AI logic if needed

        // Destroy the enemy after the death animation finishes
        // We use a coroutine to wait until the animation finishes
        StartCoroutine(WaitForDeathAnimation());
    }

    private IEnumerator WaitForDeathAnimation()
    {
        // Assuming you are getting the length of the animation in seconds
        float deathAnimationLength = 1f;  // Example length of the death animation

        yield return new WaitForSeconds(deathAnimationLength);

        // Now destroy the object
        Destroy(gameObject);
    }
}

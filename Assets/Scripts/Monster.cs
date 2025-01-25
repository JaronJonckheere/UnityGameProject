using UnityEngine;

public class Monster : MonoBehaviour
{
    public int monsterID;  // Unique ID for the monster
    public int health;  // Health of the monster

    void Start()
    {
        // Initialize monster properties
        monsterID = 1;  // You can assign unique IDs or make this dynamic
        health = 100;  // Set initial health, can be customized per monster
    }

    void Update()
    {
        // Monster behavior can be defined here, such as movement or attacks
    }

    public void TakeDamage(int damage)
    {
        // Example method for reducing health when the monster takes damage
        health -= damage;
        if (health <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        // Destroy the monster when health reaches 0
        Destroy(gameObject);
        Debug.Log("Monster died");
    }
}

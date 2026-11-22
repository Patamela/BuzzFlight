using System;
using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    
    private int maxHealth;
    private int currentHealth;

    public void InitializeHealth(int health)
    {
        maxHealth = health;
    }

    private void OnEnable()
    {
        // Initialize the current health from the ScriptableObject data
        currentHealth = maxHealth;
    }

    public void LoseHealth(int damage = 1)
    {
        currentHealth -= damage;

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        gameObject.SetActive(false);
    }
}

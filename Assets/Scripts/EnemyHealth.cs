using System;
using System.Collections;
using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    
    [SerializeField] private ParticleSystem enemyParticles;

    private int maxHealth;
    private int currentHealth;

    public void InitializeHealth(int health)
    {
        maxHealth = health;
        currentHealth = maxHealth;
    }

    public void LoseHealth(int damage = 1)
    {
        currentHealth -= damage;

        if (currentHealth <= 0)
        {
            StartCoroutine(Die());
        }
    }

    private IEnumerator Die()
    {
        enemyParticles.Play();
        yield return new WaitForSeconds(0.5f);
        gameObject.SetActive(false);
    }
}

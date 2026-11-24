using System;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class ExplosiveObject : MonoBehaviour
{   
    [SerializeField] private float explosionRadius = 5f;
    [SerializeField] LayerMask playerLayer;
    [SerializeField] ParticleSystem explosiveParticle;

    PlayerHealth playerHealth;
    EnemyManager enemyManager;

    int damageExplosive = 2;

    public void Detonate()
    {   
        
        StartCoroutine(ExplosionParticles());

        Collider[] playerCollider = Physics.OverlapSphere(transform.position, explosionRadius, playerLayer);

        foreach(Collider hit in playerCollider)
        {
            
            if (hit != null)
            {
                if (hit.CompareTag("Player"))
                {
                    playerHealth = hit.GetComponent<PlayerHealth>();
                    break;
                }
                else if (hit.CompareTag("Enemy"))
                {

                    enemyManager = hit.GetComponent<EnemyManager>();
                    enemyManager.TakeDamage(damageExplosive);
                }
                
            }
        }

        if(playerHealth != null) playerHealth.LoseHealth(damageExplosive);
        
    }

    IEnumerator ExplosionParticles()
    {   
        explosiveParticle.Play();
        yield return new WaitForSeconds(1.2f);
        Destroy(gameObject);
    }


}

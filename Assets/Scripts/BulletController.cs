using UnityEngine;

public class BulletController : MonoBehaviour
{
    [Header("Bullet Settings")]
    [SerializeField] private float bulletLifeTime = 1f;

    [Header("Particle System")]
    [SerializeField] private ParticleSystem bulletImpactParticleSystem;
    

    EnemyHealth enemyHealth;
    ExplosiveObject explosiveObject;
    PlayerHealth playerHealth;

    void OnEnable()
    {   
        // Start the bullet lifetime countdown
        Invoke(nameof(Deactivate), bulletLifeTime);
    }

    /// <summary>
    /// Desactivates the bullet
    /// </summary>
    private void Deactivate()
    {   
        gameObject.SetActive(false);
    }

    void OnTriggerEnter(Collider other)
    {
        //Deactivate the bullet when it collides with another object
        Deactivate();
        switch (other.tag)
        {
            case "Enemy":
                ActivateImpactParticleSystem();
                enemyHealth = other.GetComponent<EnemyHealth>();
                if(enemyHealth != null) enemyHealth.LoseHealth();
                break;
            case "Explosive":
                ActivateImpactParticleSystem();
                explosiveObject = other.GetComponent<ExplosiveObject>();
                if(explosiveObject != null) explosiveObject.Detonate();
                break;
            case "Player":
                ActivateImpactParticleSystem();
                /*playerHealth = other.GetComponent<PlayerHealth>();
                playerHealth.LoseHealth(enemyManager.GetDamage());*/
                break;
            case "Obstacle":
                ActivateImpactParticleSystem();
                break;
            case "Explosion":
                ActivateImpactParticleSystem();
                break;
            default:
                break;
        }
    }

    private void ActivateImpactParticleSystem()
    {
        bulletImpactParticleSystem.Play();
    }
}

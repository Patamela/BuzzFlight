using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CollisionManager : MonoBehaviour
{
    [Header("Audio Settings")]
    [SerializeField] private AudioClip landingAudioClip;

    [Header("Particle System")]
    [SerializeField] private ParticleSystem landingParticleSystem;

    AudioSource audioSource;
    PlayerHealth playerHealth;
    PlayerShooting playerShooting;
    PlayerMovement playerMovement;
    EnemyManager enemyManager;

    float delay = 1f;
    bool isControllable = true;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        playerHealth = GetComponent<PlayerHealth>();
        playerShooting = GetComponent<PlayerShooting>();
        playerMovement = GetComponent<PlayerMovement>();
    }

    void OnCollisionEnter(Collision collision)
    {
        if(!isControllable) return;

        switch (collision.gameObject.tag)
        {
            case "Start":
                Debug.Log("LaunchPad Collision Detected");
                break;
            case "Enemy":
                ManageEnemyCollision(collision);
                break;
            case "Obstacle":
                playerHealth.LoseHealth();
                playerMovement.DashInterrupted = true;
                break;
            default:
                break;
        }

        
    }

    void ManageEnemyCollision(Collision collision)
    {   
        enemyManager = collision.gameObject.GetComponent<EnemyManager>();
        if (playerMovement.IsDashing)
        {
            enemyManager.TakeDamage();
        }
        else
        {
            playerHealth.LoseHealth(enemyManager.GetDamage());
        }
    }

    void OnCollisionExit(Collision collision)
    {
        playerMovement.DashInterrupted = false;
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Point"))
        {
            int currentScore = PlayerPrefs.GetInt("MaxScore") + 100;
            PlayerPrefs.SetInt("MaxScore", currentScore);
            Destroy(other.gameObject);
        }

        if(other.CompareTag("Finish")) ManageCollision(landingAudioClip, landingParticleSystem);

        if(!other.CompareTag("Explosion")) return;
        playerHealth.LoseHealth();
    }

    /// <summary>
    /// Manages the collsion event by disabling palyer control,
    /// stopping any ongoing audio, playing the specified adio clip,
    /// playing the specified particle system,
    /// and disabling the layer movement.
    /// </summary>
    /// <param name="audioClip"></param>
    /// <param name="particleSystem"></param>
    private void ManageCollision(AudioClip audioClip, ParticleSystem particleSystem)
    {
        audioSource.Stop();
        audioSource.PlayOneShot(audioClip);
        particleSystem.Play();
    }

    private void LoadLevel()
    {
        SceneManager.LoadScene("Level1");
    }

    public void ApplyPowerUp(PowerUpScriptableObject powerUpData)
    {
        if(powerUpData.PowerUpType == "Ammo")
        {
            playerShooting.RecoverAmmo();
        }
        else if(powerUpData.PowerUpType == "Health")
        {
           playerHealth.RecoverHealth(); 
        }
    }

}

using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    [SerializeField] PlayerHealth playerHealth;
    [SerializeField] PlayerShooting playerShooting;
    [SerializeField] ParticleSystem checkpointParticle;

    bool isActivated = false;

    void OnTriggerEnter(Collider other)
    {
        if(!other.CompareTag("Player")) return;

        if (!isActivated)
        {
            playerHealth.SetSpawnPoint(transform.position);
            playerHealth.CurrentLives = playerHealth.MaxLives;
            playerShooting.CurrentAmmo = playerShooting.MaxAmmo;
            checkpointParticle.Play();
            isActivated = true;
        }
    }
}

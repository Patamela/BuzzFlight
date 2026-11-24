using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    [SerializeField] private EnemyData enemyData;

    EnemyHealth enemyHealth;

    void Awake()
    {
        enemyHealth = GetComponent<EnemyHealth>();
    }

    void Start()
    {
        Initialize();
    }

    public void Initialize()
    {
        enemyHealth.InitializeHealth(enemyData.Health);
    }

    public void TakeDamage(int damage = 1)
    {
        enemyHealth.LoseHealth(damage);
    }

    public int GetDamage()
    {
        return enemyData.Damage;
    }

}

using UnityEngine;
using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using System;

public class Ally : MonoBehaviour
{
    [Header("Detection Settings")]
    [SerializeField] private float detectionRange = 5f;

    [Header("Movement Settings")]
    [SerializeField] private float moveSpeed = 2f;
    [SerializeField] private float floatingHeight = 1f;
    [SerializeField] private float floatingDuration = 1f;
    [SerializeField] private float offset = 2f;
    [SerializeField] private float followDuration = 5f;

    [Header("Shooting Settings")]
    [SerializeField] private float bulletSpeed = 20f;
    [SerializeField] private float bulletRate = 0.25f;

    private Transform playerTransform;
    private Transform bulletSpawnPoint;
    private ObjectPool bulletPool;

    private Vector3 startPosition;
    private Tween floatingTween;

    List<Transform> enemies;
    Coroutine attackRoutine;

    float lastShotTime = 0f;
    bool hasJoined;
    bool isFollowing;

    void Awake()
    {
        enemies = new List<Transform>();
    }

    void Start()
    {
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
        startPosition = transform.position;
        bulletSpawnPoint = transform.Find("ShootPoint");
        bulletPool = GetComponent<ObjectPool>();
        StartFloting();
    }

    void Update()
    {
        float distanceToPlayer = Vector3.Distance(transform.position, playerTransform.position);

        if(!hasJoined && distanceToPlayer <= detectionRange)
        {   
            hasJoined = true;
            floatingTween.Kill();
            StartCoroutine(JoinToTeam());
        }

        if (isFollowing)
        {
            FollowPlayer();
        }
    }

    private void FollowPlayer()
    {
        Vector3 targetPosition = playerTransform.position + Vector3.up * offset;

        transform.position = Vector3.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);
    }

    void OnTriggerEnter(Collider other)
    {

        if(!other.CompareTag("Enemy")) return;
        enemies.Add(other.transform);

        if(attackRoutine == null){
            attackRoutine = StartCoroutine(AttackEnemies());
        }
    }

    void OnTriggerExit(Collider other)
    {
        if(!other.CompareTag("Enemy")) return;
        enemies.Remove(other.transform);
    }

    IEnumerator AttackEnemies()
    {   
        while (true)
        {   
            enemies.RemoveAll(e => e == null);

            if(enemies.Count == 0)
            {
                attackRoutine = null;
                yield break;
            }

            Transform target = GetClosestsEnemy();
            Shoot(target);
            yield return new WaitForSeconds(bulletRate);
        }
    }

    private Transform GetClosestsEnemy()
    {
        Transform closest = null;
        float minDistance = float.MaxValue;

        foreach(Transform enemy in enemies)
        {
            float d = (enemy.position - transform.position).sqrMagnitude;
            if(d < minDistance)
            {
                minDistance = d;
                closest = enemy;
            }
        }

        return closest;
    }

    void Shoot(Transform target)
    {
        GameObject bullet = bulletPool.GetObjectFromPool();
        bullet.transform.position = bulletSpawnPoint.position;
        bullet.transform.rotation = bulletSpawnPoint.rotation;

        Vector3 direction = (target.position - bulletSpawnPoint.position).normalized;
        bullet.transform.rotation = Quaternion.LookRotation(direction);

        Rigidbody bulletRigidbody = bullet.GetComponent<Rigidbody>();
        bulletRigidbody.linearVelocity = Vector3.zero;
        bulletRigidbody.angularVelocity = Vector3.zero;
        bulletRigidbody.linearVelocity = direction * bulletSpeed;
    }

    IEnumerator JoinToTeam()
    {
        Vector3 targetPosition = playerTransform.position + Vector3.up * offset;

        float distance = Vector3.Distance(transform.position, targetPosition);
        float duration = distance / moveSpeed;

        floatingTween = transform
                    .DOMove(targetPosition, duration)
                    .SetEase(Ease.Linear);

        yield return floatingTween.WaitForCompletion();

        isFollowing = true;

        yield return new WaitForSeconds(followDuration);

        Destroy(gameObject);
    }

    private void StartFloting()
    {
        floatingTween = transform.DOMoveY(startPosition.y + floatingHeight, floatingDuration)
            .SetLoops(-1, LoopType.Yoyo)
            .SetEase(Ease.InOutSine);
    }

    private void OnDrawGizmosSelected()
    {
        // Draw detection range
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, detectionRange);
    }
}

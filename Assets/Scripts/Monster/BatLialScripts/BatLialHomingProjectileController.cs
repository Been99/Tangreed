using System.Collections;
using UnityEngine;

public class BatLialHomingProjectileController : MonoBehaviour
{
    private Vector3 initialDirection;
    private Transform playerTransform;
    private LayerMask playerLayer;
    private MonsterAttackSO monsterAttackSO;
    private TrailRenderer trailRenderer;
    private float durationTime = 2f;
    private float homingTime = 0.6f; // 유도 시간
    private float timeElapsed = 0;
    private bool isHoming = true;

    private float homingSpeed;
    private float minHomingSpeed = 2.5f;
    private float maxHomingSpeed = 12.5f;
    private float homingSpeedChangeInterval = 0.1f;

    private void Awake()
    {
        trailRenderer = GetComponent<TrailRenderer>();
        playerLayer = LayerMask.NameToLayer("Player");
    }

    public void Initialize(Vector3 initialDirection, Transform playerTransform, MonsterAttackSO attackData)
    {
        this.initialDirection = initialDirection.normalized;
        this.playerTransform = playerTransform;
        this.monsterAttackSO = attackData;
        homingSpeed = attackData.projectileSpeed;
        StartCoroutine(ChangeHomingSpeed());
    }

    private void Update()
    {
        timeElapsed += Time.deltaTime;

        if (isHoming && playerTransform != null && timeElapsed <= homingTime)
        {
            Vector3 directionToPlayer = (playerTransform.position - transform.position).normalized;
            initialDirection = Vector3.Lerp(initialDirection, directionToPlayer, Time.deltaTime * homingSpeed).normalized;
            transform.position += initialDirection * homingSpeed * Time.deltaTime;
            transform.rotation = Quaternion.LookRotation(Vector3.forward, directionToPlayer) * Quaternion.Euler(0, 0, 90);
        }
        else
        {
            isHoming = false;
            transform.position += initialDirection * homingSpeed * Time.deltaTime;
        }

        if (timeElapsed >= durationTime)
        {
            DestroyProjectile();
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.layer == playerLayer)
        {
            HitTarget(other.gameObject);
        }
    }

    private void HitTarget(GameObject target)
    {
        HealthSystem targetHealthSystem = target.GetComponent<HealthSystem>();
        if (targetHealthSystem != null)
        {
            targetHealthSystem.ChangeHealth(-monsterAttackSO.attackDamage * 2);
        }

        DestroyProjectile();
    }

    private IEnumerator ChangeHomingSpeed()
    {
        while (timeElapsed <= homingTime)
        {
            homingSpeed = Random.Range(minHomingSpeed, maxHomingSpeed);
            yield return new WaitForSeconds(Random.Range(0.05f, homingSpeedChangeInterval));
        }
    }

    private void DestroyProjectile()
    {
        Destroy(this.gameObject);
    }
}

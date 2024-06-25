using System.Collections;
using UnityEngine;

public class BatLialAttack : MonsterAttack // 뱃리알 전용
{
    public GameObject projectilePrefab;
    public GameObject homingProjectilePrefab; // 유도탄 전용 프리팹
    public int numberOfProjectiles;
    public int maxAttackCount;
    public float spreadAngle = 360f;
    public float restDuration = 2.5f; // 휴식 시간
    public float projectileInterval = 4.5f; // 유도탄 발사 간격

    private int attackCount = 0;
    private bool isResting = false;

    protected void Start()
    {
        StartCoroutine(FireHomingProjectiles()); // 유도탄 발사 코루틴 시작
    }

    protected override void Update()
    {
        if (monsterController.healthSystem.currentHP <= 0) { return; }

        target = monsterMechanism.GetPlayerTransform();
        if (target == null) { return; }

        if (isResting) { return; }

        float attackRange = monsterStatsHandler.currentStats.monsterAttackSO.attackRange;
        float attackDelay = monsterStatsHandler.currentStats.monsterAttackSO.attackDelay;
        float distanceToPlayer = Vector2.Distance(this.transform.position, target.position);

        if (distanceToPlayer <= attackRange)
        {
            if (!monsterController.isAttacking && Time.time - timeSinceLastAttack >= attackDelay)
            {
                Attack();
                timeSinceLastAttack = Time.time;
                attackCount++;

                if (attackCount >= maxAttackCount)
                {
                    StartCoroutine(RestCoroutine());
                }
            }
        }
    }

    protected override void ApplyDamage(HealthSystem targetHealthSystem)
    {
        float angleStep = spreadAngle / numberOfProjectiles;
        float angle = 0f;
        float angleOffset = Random.Range(-90f, 90f); // 각도를 랜덤으로 틀어지게

        for (int i = 0; i < numberOfProjectiles; i++)
        {
            float currentAngle = angle + angleOffset;
            float projectileDirX = transform.position.x + Mathf.Sin((currentAngle * Mathf.PI) / 180);
            float projectileDirY = transform.position.y + Mathf.Cos((currentAngle * Mathf.PI) / 180);

            Vector3 projectileVector = new Vector3(projectileDirX, projectileDirY, 0);
            Vector3 projectileMoveDirection = (projectileVector - transform.position).normalized;

            GameObject projectile = Instantiate(projectilePrefab, transform.position, Quaternion.identity);
            MonsterProjectileController projectileController = projectile.GetComponent<MonsterProjectileController>();

            if (projectileController != null)
            {
                projectileController.Initialize(projectileMoveDirection, monsterStatsHandler.currentStats.monsterAttackSO);
            }

            angle += angleStep;
        }
    }

    private IEnumerator FireHomingProjectiles()
    {
        while (true)
        {
            if (target != null)
            {
                for (int i = 0; i < numberOfProjectiles; i++)
                {
                    float angleStep = spreadAngle / numberOfProjectiles;
                    float angle = i * angleStep;

                    float projectileDirX = transform.position.x + Mathf.Sin((angle * Mathf.PI) / 180);
                    float projectileDirY = transform.position.y + Mathf.Cos((angle * Mathf.PI) / 180);

                    Vector3 projectileVector = new Vector3(projectileDirX, projectileDirY, 0);
                    Vector3 projectileMoveDirection = (projectileVector - transform.position).normalized;

                    GameObject projectile = Instantiate(homingProjectilePrefab, transform.position, Quaternion.identity);
                    BatLialHomingProjectileController projectileController = projectile.GetComponent<BatLialHomingProjectileController>();

                    if (projectileController != null)
                    {
                        projectileController.Initialize(projectileMoveDirection, target, monsterStatsHandler.currentStats.monsterAttackSO);
                    }

                    yield return new WaitForSeconds(projectileInterval);
                }
            }
            else
            {
                yield return null;
            }
        }
    }

    private IEnumerator RestCoroutine()
    {
        isResting = true;
        yield return new WaitForSeconds(restDuration);
        attackCount = 0;
        isResting = false;
    }

    protected override void Attack()
    {
        base.Attack();
    }
}
using UnityEngine;

public class MonsterRangedAttack : MonsterAttack // 원거리 전용
{
    public GameObject projectilePrefab;

    protected override void ApplyDamage(HealthSystem targetHealthSystem)
    {
        GameObject projectile = Instantiate(projectilePrefab, transform.position, Quaternion.identity);
        MonsterProjectileController projectileController = projectile.GetComponent<MonsterProjectileController>();

        if (projectileController != null)
        {
            Vector3 direction = (target.position - transform.position).normalized;
            projectileController.Initialize(direction, monsterStatsHandler.currentStats.monsterAttackSO);
        }
    }
}
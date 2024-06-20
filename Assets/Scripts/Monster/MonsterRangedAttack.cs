using UnityEngine;

public class MonsterRangedAttack : MonsterAttack
{
    public GameObject projectilePrefab;

    protected override void Attack()
    {
        base.Attack();

        GameObject projectile = Instantiate(projectilePrefab, transform.position, Quaternion.identity);
        MonsterProjectileController projectileController = projectile.GetComponent<MonsterProjectileController>();

        if(projectileController != null)
        {
            Vector3 direction = (target.position - transform.position).normalized;
            projectileController.Initialize(direction, monsterStatsHandler.currentStats.monsterAttackSO);
        }
    }
}
using UnityEngine;

public class MonsterMeleeAttack : MonsterAttack // 근거리 전용
{
    protected override void ApplyDamage(HealthSystem targetHealthSystem)
    {
        targetHealthSystem.ChangeHealth(-monsterStatsHandler.currentStats.monsterAttackSO.attackDamage);
    }
}
using UnityEngine;

public class MonsterAttack : MonoBehaviour
{
    protected Transform target;
    protected MonsterController monsterController;
    protected MonsterMechanism monsterMechanism;
    protected MonsterStatsHandler monsterStatsHandler;
    protected float timeSinceLastAttack = 0;

    protected virtual void Awake()
    {
        monsterController = GetComponent<MonsterController>();
        monsterMechanism = GetComponent<MonsterMechanism>();
        monsterStatsHandler = GetComponent<MonsterStatsHandler>();
    }

    protected virtual void Update()
    {
        target = monsterMechanism.GetPlayerTransform();

        if (target == null) { return; }

        float attackRange = monsterStatsHandler.currentStats.monsterAttackSO.attackRange;
        float attackDelay = monsterStatsHandler.currentStats.monsterAttackSO.attackDelay;
        float distanceToPlayer = Vector2.Distance(this.transform.position, target.position);

        if (distanceToPlayer <= attackRange)
        {
            if (!monsterController.isAttacking && Time.time - timeSinceLastAttack >= attackDelay)
            {
                Attack();
                timeSinceLastAttack = Time.time;
            }
        }
    }

    protected virtual void Attack()
    {
        monsterController.OnAttack(monsterStatsHandler.currentStats.monsterAttackSO);
        Invoke("EndAttack", monsterStatsHandler.currentStats.monsterAttackSO.attackDelay);
    }

    protected virtual void EndAttack()
    {
        monsterController.isAttacking = false;
    }
}
using UnityEngine;

public class MonsterAttack : MonoBehaviour
{
    private Transform target;
    private MonsterController monsterController;
    private MonsterMechanism monsterMechanism;
    private MonsterStatsHandler monsterStatsHandler;
    private float timeSinceLastAttack = 0;

    private void Awake()
    {
        monsterController = GetComponent<MonsterController>();
        monsterMechanism = GetComponent<MonsterMechanism>();
        monsterStatsHandler = GetComponent<MonsterStatsHandler>();
    }

    private void Update()
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

    private void Attack()
    {
        monsterController.OnAttack(monsterStatsHandler.currentStats.monsterAttackSO);
        Invoke("EndAttack", monsterStatsHandler.currentStats.monsterAttackSO.attackDelay);
    }

    private void EndAttack()
    {
        monsterController.isAttacking = false;
    }
}
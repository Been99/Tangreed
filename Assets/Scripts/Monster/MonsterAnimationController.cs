using System.Collections;
using UnityEngine;

public class MonsterAnimationController : MonoBehaviour
{
    private Animator animator;
    private HealthSystem healthSystem;
    private MonsterController monsterController;

    private static readonly float magnitudeThreshold = 0.1f;
    private static readonly int isMovingHash = Animator.StringToHash("isMoving");
    private static readonly int attackTriggerHash = Animator.StringToHash("attack");
    private static readonly int hitTriggerHash = Animator.StringToHash("hit");
    private static readonly int deadTriggerHash = Animator.StringToHash("dead");

    private bool isDead = false;
    private void Awake()
    {
        animator = GetComponentInChildren<Animator>();
        healthSystem = GetComponent<HealthSystem>();
        monsterController = GetComponent<MonsterController>();
    }

    private void Start()
    {
        monsterController.OnMoveEvent += Moving;
        monsterController.OnAttackEvent += Attacking;

        if (healthSystem != null)
        {
            healthSystem.OnDamageEvent += Hitting;
            healthSystem.OnDeathEvent += Death;
        }
    }

    private void Moving(Vector2 direction)
    {
        if (isDead) { return; }
        animator.SetBool(isMovingHash, direction.magnitude > magnitudeThreshold);
    }

    private void Attacking(MonsterAttackSO monsterAttackSO)
    {
        if (isDead) { return; }
        animator.SetTrigger(attackTriggerHash);
    }

    private void Hitting()
    {
        if (isDead) { return; }
        animator.SetTrigger(hitTriggerHash);
    }

    private void Death()
    {
        if (isDead) { return; }

        animator.SetBool(isMovingHash, false);
        animator.SetTrigger(deadTriggerHash);

        StartCoroutine(DeactivateAfterDelay(2f));
    }

    private IEnumerator DeactivateAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        Destroy(gameObject);
    }
}

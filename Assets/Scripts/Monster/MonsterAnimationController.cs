using System.Collections;
using UnityEngine;

public class MonsterAnimationController : MonoBehaviour
{
    protected Animator animator;
    protected HealthSystem healthSystem;
    protected MonsterController monsterController;

    private static readonly float magnitudeThreshold = 0.1f;
    protected static readonly int isMovingHash = Animator.StringToHash("isMoving");
    protected static readonly int attackTriggerHash = Animator.StringToHash("attack");
    protected static readonly int hitTriggerHash = Animator.StringToHash("hit");
    protected static readonly int deadTriggerHash = Animator.StringToHash("dead");

    protected bool isDead = false;

    private void Awake()
    {
        animator = GetComponentInChildren<Animator>();
        healthSystem = GetComponent<HealthSystem>();
        monsterController = GetComponent<MonsterController>();
    }

    protected virtual void Start()
    {
        SubscribeToEvents();
    }

    protected virtual void SubscribeToEvents()
    {
        monsterController.OnMoveEvent += Moving;
        monsterController.OnAttackEvent += Attacking;

        if (healthSystem != null)
        {
            healthSystem.OnDamageEvent += Hitting;
            healthSystem.OnDeathEvent += Death;
        }
    }

    protected void Moving(Vector2 direction)
    {
        if (isDead) { return; }
        animator.SetBool(isMovingHash, direction.magnitude > magnitudeThreshold);
    }

    protected void Attacking(MonsterAttackSO monsterAttackSO)
    {
        if (isDead) { return; }
        animator.SetTrigger(attackTriggerHash);
    }

    protected void Hitting()
    {
        if (isDead) { return; }
        animator.SetTrigger(hitTriggerHash);
    }

    protected virtual void Death()
    {
        if (isDead) { return; }

        isDead = true;
        animator.SetBool(isMovingHash, false);
        animator.SetTrigger(deadTriggerHash);

        StartCoroutine(DeactivateAfterDelay(2f));
    }

    protected IEnumerator DeactivateAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        Destroy(gameObject);
    }
}

using System;
using System.Collections;
using UnityEngine;

public class MonsterController : MonoBehaviour
{
    [HideInInspector] public HealthSystem healthSystem;

    public Action<Vector2> OnMoveEvent;
    public Action<MonsterAttackSO> OnAttackEvent;
    public Action<MonsterController> OnDeathEvent;

    public bool isAttacking = false;

    private void Awake()
    {
        healthSystem = GetComponent<HealthSystem>();
    }

    private void Start()
    {
        healthSystem.OnDeathEvent += OnDeath;
    }

    private void FixedUpdate()
    {
        if (healthSystem == null) { return; }
        if (healthSystem.currentHP <= 0) { return; }
    }

    public void OnMove(Vector2 direction)
    {
        OnMoveEvent?.Invoke(direction);
    }

    public void OnAttack(MonsterAttackSO monsterAttackSO)
    {
        isAttacking = true;
        OnAttackEvent?.Invoke(monsterAttackSO);
    }

    public void OnDeath()
    {
        isAttacking = false;

        OnDeathEvent?.Invoke(this);
    }
}

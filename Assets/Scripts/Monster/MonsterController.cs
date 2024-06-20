using System;
using UnityEngine;

public class MonsterController : MonoBehaviour
{
    public Action<Vector2> OnMoveEvent;
    public Action<MonsterAttackSO> OnAttackEvent;

    public void OnMove(Vector2 direction)
    {
        OnMoveEvent?.Invoke(direction);
    }

    public void OnAttack(MonsterAttackSO monsterAttackSO)
    {
        OnAttackEvent?.Invoke(monsterAttackSO);
    }
}

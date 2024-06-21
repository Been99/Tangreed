using UnityEngine;

[CreateAssetMenu(fileName = "monsterAttackSO", menuName = "Tangreed/MonsterAttacks", order = 2)]
public class MonsterAttackSO : ScriptableObject
{
    [Header("Attack Info")]
    public int attackDamage;
    public float attackDelay;
    public float attackRange;
    public float projectileSpeed;
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "AttackSO", menuName = "Player/AttackSO", order = 0)]
public class PlayerAttackSO : ScriptableObject
{
    [Header("Attack Info")]
    public float size;
    public float delay;
    public int power;
    public float speed;
    public LayerMask target;
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "RangedAttackSO", menuName = "Player/RangedAttackSO", order = 1)]

public class PlayerRangedAttackSO : PlayerAttackSO
{
    [Header("Ranged Attack Data")]
    public string bulletNameTag;
    public float duration;
}

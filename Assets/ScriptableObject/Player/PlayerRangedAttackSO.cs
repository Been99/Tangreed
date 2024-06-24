using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PlayerRangedAttack", menuName = "SO/Player/New PlayerRangedAttack", order = 0)]

public class PlayerRangedAttackSO : PlayerAttackSO
{
    [Header("Ranged Attack Data")]
    public string bulletNameTag;
    public float duration;
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PlayerAttack", menuName = "SO/Player/New PlayerAttack", order = 0)]
public class PlayerAttackSO : MonoBehaviour
{
    [Header("Attack Info")] //정보가 많이 들어갈 것이기 때문에 카테고리를 만들어 주는 것이 좋음
    public float size;
    public float delay;
    public int power;
    public float speed;
    public LayerMask target;
}

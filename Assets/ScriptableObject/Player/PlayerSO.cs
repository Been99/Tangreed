using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Player", menuName = "SO/New Player", order = 0)]
public class PlayerSO : ScriptableObject
{
    [Header("Stats")]
    public string Name; //이름
    public float CureentHealth; //체력
    public float BoostSpeed;
    [Range(0f, 100f)] public float Critical;
    public int Damage;
    public int Defense;
    public int Gold;
    public float Dashcount = 2f;
    public float DashMax = 2f;
    public float Dashcool = 1f;
}

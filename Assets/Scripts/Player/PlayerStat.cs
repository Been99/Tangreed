using UnityEngine;


[System.Serializable]
public class PlayerStat
{
    [Header("Stats")]
    public string Name; //이름
    [Range(0f, 100f)] public float Critical;
    public int Damage;
    public int Defense;
    public int Gold;
    public float Dashcount = 2f;
    public float DashMax = 2f;
    public float Dashcool = 1f;

    public StatsChangeType statsChangeType;
    [Range(0, 100)] public int maxHealth;
    [Range(0f, 25f)] public float speed;
    public PlayerAttackSO playerSO;
}

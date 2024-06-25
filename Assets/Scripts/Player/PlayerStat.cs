using UnityEngine;


[System.Serializable]
public class PlayerStat
{
    [Header("Stats")]
    public StatsChangeType statsChangeType;
    public float Damage;
    public float Defense;
    [Range(-10, 100)] public int maxHealth;
    [Range(-10f, 100f)] public float Critical;
    [Range(-10f, 25f)] public float speed;
    public float Dashcount = 2f;
    public float DashMax = 2f;
    public float Dashcool = 1f;


}

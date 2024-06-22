using UnityEngine;


[System.Serializable]
public class PlayerStat
{
    public StatsChangeType statsChangeType;
    [Range(0, 100)] public int maxHealth;
    [Range(0f, 25f)] public float speed;
    public PlayerSO playerSO;
}

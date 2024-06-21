using System.Collections.Generic;
using UnityEngine;

public class PlayerStatHandler : MonoBehaviour
{
    [SerializeField] private PlayerStat baseStat;
    public PlayerStat CurrentStat { get; private set; }
    public List<PlayerStat> statModifiers = new List<PlayerStat>();

    private readonly float MinCritical = 5f; // 공격 딜레이 최소값
    private readonly float MinAttackPower = 5f; // 공격력 최소값
    private readonly float MinAttackSize = 1f; // 공격 크기 최소값
    private readonly float MinAttackSpeed = 1f; // 공격 스피드 최소값
    private readonly float MinSpeed = 1f; // 속도 최소값
    private readonly int MinMaxHealth = 50; // 최대 체력의 최소값

    private void Awake()
    {
        // CurrentStat 초기화
        if (baseStat != null)
        {
            CurrentStat = gameObject.AddComponent<PlayerStat>();
            CurrentStat.statsChangeType = baseStat.statsChangeType;
            CurrentStat.maxHealth = baseStat.maxHealth;
            CurrentStat.speed = baseStat.speed;
            CurrentStat.playerSO = baseStat.playerSO;
        }
    }
    void Update()
    {

    }
}

using UnityEngine;

public class MonsterStatsHandler : MonoBehaviour
{
    public MonsterStats baseStats;
    public MonsterStats currentStats { get; private set; }

    private void Awake()
    {
        InitializeMonsterStats();
    }

    public void InitializeMonsterStats()
    {
        if (baseStats == null) {  return; }

        MonsterAttackSO monsterAttackSO = Instantiate(baseStats.monsterAttackSO);

        currentStats = new MonsterStats
        {
            monsterName = baseStats.monsterName,
            maxHP = baseStats.maxHP,
            moveSpeed = baseStats.moveSpeed,
            experienceReward = baseStats.experienceReward,
            monsterAttackSO = monsterAttackSO,
        };
    }
}
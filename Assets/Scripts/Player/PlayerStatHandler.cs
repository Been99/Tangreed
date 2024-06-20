using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStatHandler : MonoBehaviour
{
    [SerializeField] private PlayerStat baseStat;
    public PlayerStat CurrentStat { get; private set; } = new();
    public List<PlayerStat> statModifiers = new List<PlayerStat>();


    private readonly float MinCritical = 5f; //공격 딜래이 최소값
    private readonly float MinAttackPower = 5f; //공격력 최소값
    private readonly float MinAttackSize = 1f; //공격 크기 최소값
    private readonly float MinAttackSpeed = 1f; // 공격 스피드 최소값
    private readonly float MinSpeed = 1f; //속도 최소값
    private readonly int MinMaxHealth = 50; // 최대 체력의 최소값
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

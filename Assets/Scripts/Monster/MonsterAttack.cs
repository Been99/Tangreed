using UnityEngine;

public class MonsterAttack : MonoBehaviour
{
    private MonsterController monsterController;
    private MonsterMechanism monsterMechanism;
    private MonsterStatsHandler monsterStatsHandler;
    private Transform target;
    private float timeSinceLastAttack = 0;

    private void Awake()
    {
        monsterController = GetComponent<MonsterController>();
        monsterMechanism = GetComponent<MonsterMechanism>();
        monsterStatsHandler = GetComponent<MonsterStatsHandler>();
    }

    private void Start()
    {
        target = monsterMechanism.playerLayer.
    }
}
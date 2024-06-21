using System;
using UnityEngine;

public class HealthSystem : MonoBehaviour
{
    private PlayerStatHandler playerStatHandler;
    private MonsterStatsHandler monsterStatsHandler;

    public bool isDead = false;

    public event Action OnDamageEvent;
    public event Action OnHealEvent;
    public event Action OnDeathEvent;

    public int currentHP { get; private set; }

    public int maxHP
    {
        get
        {
            if (playerStatHandler != null) { return playerStatHandler.CurrentStat.maxHealth; }
            else if (monsterStatsHandler != null) { return monsterStatsHandler.currentStats.maxHP; }
            else return 0;
        }
    }

    public void Awake()
    {
        playerStatHandler = GetComponent<PlayerStatHandler>();
        monsterStatsHandler = GetComponent<MonsterStatsHandler>();
    }

    private void Start()
    {
        currentHP = maxHP;
    }

    public bool ChangeHealth(int changeHP) // 체력 변경 처리의 핵심이 되는 메서드
    {
        int oldHP = currentHP;
        currentHP += changeHP;
        currentHP = Mathf.Clamp(currentHP, 0, maxHP);

        int damageSize = Mathf.Abs(changeHP);

        if (isDead) { return false; }

        if (changeHP < 0)
        {
            OnDamageEvent?.Invoke();
            if (playerStatHandler != null)
            {
                Debug.Log($"플레이어 피격. 원래 체력: {oldHP}, 받은데미지: {damageSize}, 현재 체력: {currentHP}");
            }
            else if (monsterStatsHandler != null)
            {
                Debug.Log($"몬스터 피격. 원래 체력: {oldHP}, 받은데미지: {damageSize}, 현재 체력: {currentHP}");
            }
        }

        if (changeHP >= 0)
        {
            OnHealEvent?.Invoke();
            if (playerStatHandler != null)
            {
                Debug.Log($"플레이어 회복. 원래 체력: {oldHP}, 회복량: {damageSize}, 현재 체력: {currentHP}");
            }
            else if (monsterStatsHandler != null)
            {
                Debug.Log($"몬스터 회복. 원래 체력: {oldHP}, 회복량: {damageSize}, 현재 체력: {currentHP}");
            }
        }

        if (currentHP <= 0)
        {
            currentHP = 0;
            CallDeath();

            return true;
        }

        return true;
    }

    public void CallDeath()
    {
        if (isDead) return;

        isDead = true;
        OnDeathEvent?.Invoke();

        if (playerStatHandler != null)
        {
            Debug.Log("플레이어 사망");
        }
        else if (monsterStatsHandler != null)
        {
            Debug.Log("몬스터 사망");
        }
    }

    public void ResetHealth() // 풀피로 만들고 싶을때 활용하시면 될 듯 합니다!
    {
        currentHP = maxHP;
        isDead = false;
    }
}

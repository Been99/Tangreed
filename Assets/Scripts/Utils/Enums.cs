
//규칙1. enum 이름 앞에 소문자e 붙이기.
//규칙2. enum의 원소들은 대문자로 만들기.

public enum StatsChangeType
{
    Add, //0
    Multiple, //1
    Override //2
}

public enum EItemType
{
    Weapon,
    Defend,
    Accessory,
    Food,
    Count
}

public enum EItemStat
{
    Health, // HP
    Strength, // 공격력
    Defensive, // 방어력
    AttackSpeed, // 공속
    MovingSpeed, // 이속
    CriticalHit, // 치명타
    Count
}

// TODO : 던그리도와 같이 배고픔 구현 할건지 여부 확인 필요함
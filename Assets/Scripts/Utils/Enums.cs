
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
    Food,
    Count
}

public enum EItemStat
{
    Health,
    Stemina,
    Count
    // TODO : 우선 두가지만 설정, 어떤 스탯이 더 필요한지 확인 필요함
}

// TODO : 던그리도와 같이 배고픔 구현 할건지 여부 확인 필요함
// 공격력, 방어력, 공속, hp, 이속, 치명타
// 위의 내용은 기본적으로 구현될 수 있도록 하기
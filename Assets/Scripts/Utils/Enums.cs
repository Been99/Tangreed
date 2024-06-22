
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
    armor,
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

// DungeonSpawner에서 사용
public enum DOORDIRECTION
{
    NoData = -1,
    Up,
    Right,
    Down,
    Left,
    Max
}

// StageData에서 사용
public enum MAPTYPE
{
    Normal,
    Restaurant,
    MapTypeMax
}

// MapManager에서 사용
public enum STAGE
{
    Stage1,
    // 추후 개발확장 시 추가
    StageMax
}

public enum ROOMTYPE
{
    Start,
    Restaurant,
    End,
    Boss,
    Normal,
    Max
}

public enum ROOMCLASS
{
    Small,
    Medium,
    Large
}

// Door에서 사용
public enum DOORTYPE
{
    Up = 0,
    Down,
    Right,
    Left,
    DoorMax
}

// BaseStage에서 사용
public enum TILEELEMENT
{
    BackGround,
    Wall,
    Movable,
    Door,
    ElementMax
}

public enum CHEST
{
    Bronze,
    Silver,
    Gold,
    Platinum,
    Boss,
    ChestMax
}

// TODO : 던그리도와 같이 배고픔 구현 할건지 여부 확인 필요함
// 공격력, 방어력, 공속, hp, 이속, 치명타
// 위의 내용은 기본적으로 구현될 수 있도록 하기
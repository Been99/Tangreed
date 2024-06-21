using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.Tilemaps;

// 모든 던전 방들의 가장 최상위 클래스
// 방의 모든 요소들(문, 함정, 몬스터, 스폰 포인트, 상자스폰 등)을 관리하고,
// 방이 시작되고 클리어되는 것에 맞춰 통제

public class BaseStage : MonoBehaviour
{
    // 던전에서 드랍되는 상자
    public GameObject[] chestPrefabs;
    public List<GameObject> chestList;

    // 몬스터 스폰 포인트
    //public MonsterSpawnPoint[] spawnPoint;

    // 자신의 모든 타일들을 벽, 배경, 빈 공간인지에 따라서 가지고 있는다.
    public Tilemap[] tiles;

    public Door[] door;

    // 각 방들의 연결 정보
    public LinkedData stageLinkedData;

    // 방들의 위치정보
    public Transform startPos;
    public Vector3Int startIndex;
    public Transform endPos;
    public Vector3Int endIndex;

    public ROOMTYPE type;
    public ROOMCLASS sizeClass;

    // 방에대한 변수들
    public GameObject playerObj;
    public int nowFloor = 0;
    public int stageNum = -1;
    public bool roomLocked; // 몬스터를 다 잡으면 Lock이 풀린다.
    public Vector2 RoomSize;
    public Transform bottomLeft;
    public Transform topRight;
    public Rect roomActiveArea;
    public Rect roomArea;
    public Vector3 centerPos = new Vector3(-3000f, -3000f);
    public int maxX;
    public int maxY;

    public int[] roomInfo;


    // 방 상태 변수, 프로퍼티
    public bool roomIsClear;
    public bool nowSpawned;

    [SerializeField] private bool nowPlayerEnter = false;
    [SerializeField] private bool isSearched = false;

    public bool IsSearched
    {
        get { return isSearched; }
        set { isSearched = value; }
    }

    public bool NowPlayerEnter
    {
        get { return nowPlayerEnter; }
        set
        {
            nowPlayerEnter = value;
            if (nowPlayerEnter)
            {
                if (IsSearched == false)
                {
                    IsSearched = true;
                }
                // TODO :: 미니맵 관련 내용 맵개발 완료 후 개발
                //MiniMap.Instance.MinimapInfoSetting(roomInfo, maxX, maxY, bottomLeft, topRight, playerObj.transform);
            }
        }
    }

    public float updateSecond = 0.1f;
    private float myTime;

    private void Awake()
    {
        playerObj = GameObject.Find("Player");
        bottomLeft = transform.Find("BottomLeft");
        topRight = transform.Find("TopRight");
        tiles = new Tilemap[(int)TILEELEMENT.ElementMax];
        Tilemap[] temp = GetComponentsInChildren<Tilemap>();
        foreach (var i in temp)
        {
            if (i.gameObject.layer == LayerMask.NameToLayer("Wall"))
            {
                tiles[(int)TILEELEMENT.Wall] = i;
            }
            else if (i.gameObject.layer == LayerMask.NameToLayer("BackGround"))
            {
                tiles[(int)TILEELEMENT.BackGround] = i;
            }
            else if (i.gameObject.layer == LayerMask.NameToLayer("Moveable"))
            {
                tiles[(int)TILEELEMENT.Movable] = i;
            }
        }
        //spawnPoint = GetComponentsInChildren<MonsterSpawnPoint>();
        LoadMapInfo();
        LoadDoors();
    }

    int count = 0;
    private void Update()
    {
        CheckRoomState();
       
        // TODO :: 상자 나오는 로직 작성
        
    }

    public void LoadDoors()
    {
        // enum DOORTYPE 의 최대치로 배열 생성 후 null로 초기화
        door = new Door[(int)DOORTYPE.DoorMax];
        for (DOORTYPE i = DOORTYPE.Up; i < DOORTYPE.DoorMax; i++)
        {
            door[(int)i] = null;
        }

        // Door 컴포넌트 카져오기
        Door[] temp = GetComponentsInChildren<Door>();

        // 각 Door 컴포넌트를 배열에 설정 후 비활성화
        foreach (var i in temp)
        {
            door[(int)i.type] = i;
            i.gameObject.SetActive(false);
        }
    }

    // 자신의 아래에 있는 문들을 받아온다
    public void InitSetting()
    {
        Vector2 temp1 = bottomLeft.position + new Vector3(2, 1, 0);
        Vector2 temp2 = topRight.position + new Vector3(-2, -1, 0);
        roomActiveArea.x = temp1.x;
        roomActiveArea.y = temp1.y;
        roomActiveArea.width = temp2.x - temp1.x;
        roomActiveArea.height = temp2.y - temp1.y;


        roomArea.x = bottomLeft.position.x;
        roomArea.y = bottomLeft.position.y;
        roomArea.width = topRight.position.x - bottomLeft.position.x;
        roomArea.height = topRight.position.y - bottomLeft.position.y;

        centerPos = (topRight.position - bottomLeft.position) * 0.5f;

        //링크정보가 있는데 해당 위치에 문이 없으면 해당 방을 다른 방이랑 교체 한다.
        //링크정보가 있고 해당 방향에 문도 존재하면 문을 만들어 준다.
        //문을 활성화 할때 맵정보에 문도 집어넣어 준다.
        if (stageLinkedData.rightMap != null)
        {
            if (door[(int)DOORTYPE.Right] != null)
            {
                door[(int)DOORTYPE.Right].gameObject.SetActive(true);
                door[(int)DOORTYPE.Right].CreateDoor(this.gameObject);
            }
        }
        if (stageLinkedData.leftMap != null)
        {
            if (door[(int)DOORTYPE.Left] != null)
            {
                door[(int)DOORTYPE.Left].gameObject.SetActive(true);
                door[(int)DOORTYPE.Left].CreateDoor(this.gameObject);
            }
        }
        if (stageLinkedData.upMap != null)
        {
            if (door[(int)DOORTYPE.Up] != null)
            {
                door[(int)DOORTYPE.Up].gameObject.SetActive(true);
                door[(int)DOORTYPE.Up].CreateDoor(this.gameObject);
            }
        }
        if (stageLinkedData.downMap != null)
        {
            if (door[(int)DOORTYPE.Down] != null)
            {
                door[(int)DOORTYPE.Down].gameObject.SetActive(true);
                door[(int)DOORTYPE.Down].CreateDoor(this.gameObject);
            }
        }

        if (type == ROOMTYPE.Start)
        {

            startPos = transform.Find($"{MapManager.Instance.NowStage}StartGate");

            if (playerObj != null)
            {
                playerObj.transform.position = startPos.position;
                NowPlayerEnter = true;
            }
        }
        else if (type == ROOMTYPE.End)
        {
            endPos = transform.Find($"{MapManager.Instance.NowStage}EndGate");
        }
    }

    // 문의 존재여부 확인
    public bool IsDoorExist(DOORTYPE type)
    {
        if (door[(int)type] == null)
        {
            return false;
        }
        else
        {
            return true;
        }

    }

    // 방의 좌하단과 우상단 좌표를 사용하여 방의 크기를 계산하고, 각 위치에 타일 정보 기록
    public void LoadMapInfo()
    {
        RaycastHit2D[] hit;

        // 방의 좌하단 좌표(월드 좌표를 타일맵 셀 좌표로 변환)
        Vector3Int bottomleftindex = tiles[(int)TILEELEMENT.Wall].WorldToCell(bottomLeft.position);
        Debug.Log($"{name}번방 로드 시작 bottomleft = {bottomLeft.position.x},{bottomLeft.position.y},{bottomLeft.position.z}");

        // 방의 우상단 좌표
        Vector3Int topRightindex = tiles[(int)TILEELEMENT.Wall].WorldToCell(topRight.position);
        Debug.Log($"{name}번방 로드 시작 topright = {topRight.position.x},{topRight.position.y},{topRight.position.z}");

        // 방의 크기를 계산하고 roomInfo 배열 갱신
        int max_X = topRightindex.x - bottomleftindex.x + 1;
        maxX = max_X;
        int max_Y = topRightindex.y - bottomleftindex.y + 1;
        maxY = max_Y;
        roomInfo = new int[maxX * maxY];

        // 각 타일의 중심을 월드좌표로 가져와서 레이캐스트 수행
        // roomInfo 배열에 각 타일의 정보 기록
        Vector3 temp = tiles[(int)TILEELEMENT.Wall].GetCellCenterWorld(bottomleftindex);
        for (int x = 0; x < maxX; x++)
        {
            for (int y = 0; y < maxY; y++)
            {
                temp.x = bottomLeft.position.x + x;
                temp.y = bottomLeft.position.y + y + 0.3f;

                hit = Physics2D.RaycastAll(temp, new Vector2(1, 1), 0f);

                roomInfo[x + (y * maxX)] = 0;
                foreach (var i in hit)
                {
                    if (i)
                    {
                        if (i.transform.gameObject.layer == LayerMask.NameToLayer("Wall"))
                        {
                            roomInfo[x + (y * maxX)] = (int)TILEELEMENT.Wall;
                        }
                        else if (i.transform.gameObject.layer == LayerMask.NameToLayer("Moveable"))
                        {
                            roomInfo[x + (y * maxX)] = (int)TILEELEMENT.Movable;
                            break;
                        }
                    }
                }

            }
        }
    }


    public void CheckRoomState()
    {
        if (NowPlayerEnter)
        {
            if (myTime <= Time.time)
            {
                myTime = Time.time + updateSecond;
                //Debug.Log("들어옴");

                //아직 몬스터들이 스폰되지 않았거나 방이 클리어되지 않았을때 캐릭터의 입장을 검사한다.
                if (nowSpawned == false && roomIsClear == false)
                {
                    //플레이어가 방 영역 안으로 들어오면 해당 문을 닫고 해당방의 몬스터들을 소환한다.
                    //이미 몬스터들이 다 죽었거나 방이 클리어 되어 있거나 몬스터가 없는 방이면 그냥 그대로 둔다. 
                    if (roomActiveArea.Contains(playerObj.transform.position))
                    {
                        RoomStart();
                    }
                }

                //몹들이 스폰됐을때는 몬스터가 다 죽었는지를 검사한다.
                //if (nowSpawned == true)
                //{
                //    if (this.type != ROOMTYPE.Boss)
                //    {
                //        if (SpawnManager.Instance.NowSpawndList.Count <= 0)
                //        {
                //            //스폰된 몬스터 전부 죽음
                //            RoomClear();
                //        }
                //    }
                //    else
                //    {
                //        if (BossManager.Instance.cur_Boss.isDie)
                //        {
                //            RoomClear();
                //        }
                //    }
                //}
            }
        }
    }

    //플레이어가 방에 들어왔을때 방을 시작 해준다.
    // TODO :: 몬스터 spawnPoint 넣기
    public void RoomStart()
    {

        //if (spawnPoint.Length != 0 && roomIsClear == false)
        if(roomIsClear == false)
        {
            //nowSpawned = true;
            for (int i = 0; i < door.Length; i++)
            {
                if (door[i] != null)
                {
                    //활성화 되어있는 문들을 닫아주고 몬스터들을 스폰한다.
                    if (door[i].gameObject.activeSelf)
                    {
                        door[i].NowDoorLocked = true;
                    }
                }

            }
          
            //몬스터들을 스폰 시켜준다.
            //if (spawnPoint[0].type != SpawnManager.MonsterType.Boss)
            //{
            //    for (int j = 0; j < spawnPoint.Length; j++)
            //    {
            //        SpawnManager.Instance.Spawn(spawnPoint[j].type, spawnPoint[j].transform);
            //        //MonsterCount++;
            //    }
            //}
            //else
            //{
            //    BossManager.Instance.Boss_Start = true;
            //}
            
            //몬스터에 정보를 넘겨준다.
            //Minimap.Instance.CreateMonsterTile(SpawnManager.Instance.NowSpawndList);
        }

        //스폰될 몬스터가 없는 방은 바로 클리어 해준다.
        //if (spawnPoint.Length <= 0)
        //{
        //    roomIsClear = true;
        //}

    }

    //방이 클리어 되었을때 다시 텔레포트랑 문들을 활성화 시켜주고 방의 클래스에 따라 확률로 상자를 스폰해준다.
    public void RoomClear()
    {
        if (!roomIsClear)
        {
            roomIsClear = true;
            for (int i = 0; i < door.Length; i++)
            {
                if (door[i] != null)
                {
                    //활성화 되어있는 문들을 다시 열어준다.
                    if (door[i].gameObject.activeSelf)
                    {
                        door[i].NowDoorLocked = false;
                    }
                }
            }
            // 상자를 소환한다
            //ChestSpawn();
        }

    }

    //방이 클리어 되었을때 해당 방의 타입과 크기에 따라 상자를 스폰 해준다. 
    //public void ChestSpawn()
    //{
    //    int per; //해당 방에 스폰될 확률
    //    int rnd;
    //    int Crnd;
    //    int[] CPer = new int[(int)CHEST.ChestMax];
    //    for (int i = 0; i < CPer.Length; i++)
    //    {
    //        CPer[i] = 0;
    //    }

    //    if (type != ROOMTYPE.Boss)
    //    {
    //        if (sizeClass == ROOMCLASS.Small)
    //        {
    //            per = 20;
    //            CPer[(int)CHEST.Bronze] = 85;
    //            CPer[(int)CHEST.Silver] = 15;
    //            CPer[(int)CHEST.Gold] = 5;
    //        }
    //        else if (sizeClass == ROOMCLASS.Large)
    //        {
    //            per = 60;
    //            CPer[(int)CHEST.Bronze] = 70;
    //            CPer[(int)CHEST.Silver] = 20;
    //            CPer[(int)CHEST.Gold] = 10;
    //            CPer[(int)CHEST.Platinum] = 15;
    //        }
    //        else
    //        {
    //            per = 100;
    //            CPer[(int)CHEST.Bronze] = 40;
    //            CPer[(int)CHEST.Silver] = 35;
    //            CPer[(int)CHEST.Gold] = 25;
    //            CPer[(int)CHEST.Platinum] = 35;
    //        }


    //        rnd = Random.Range(0, 100);

    //        if (rnd <= per)
    //        {
    //            Crnd = UnityEngine.Random.Range(0, 100);
    //            for (int i = (int)CHEST.Gold; i >= (int)CHEST.Bronze; i--)
    //            {
    //                if (Crnd <= CPer[i])
    //                {
    //                    GameObject obj = Instantiate(chestPrefabs[i]);
    //                    obj.transform.position = spawnPoint[spawnPoint.Length - 1].transform.position;
    //                    chestList.Add(obj);
    //                    break;
    //                }
    //            }
    //            if (Crnd <= CPer[(int)CHEST.Platinum])
    //            {
    //                GameObject obj = Instantiate(chestPrefabs[(int)CHEST.Platinum]);
    //                obj.transform.position = spawnPoint[spawnPoint.Length - 1].transform.position + new Vector3(1, 0, 0);
    //                chestList.Add(obj);
    //            }

    //        }
    //    }
    //    else
    //    {
    //        GameObject obj = Instantiate(chestPrefabs[(int)CHEST.Boss]);
    //        obj.transform.position = spawnPoint[spawnPoint.Length - 1].transform.position;
    //        chestList.Add(obj);
    //    }
    //}

    public bool IsEnterPlayArea()
    {
        bool flag = false;
        Vector3 pos = playerObj.transform.position;
        if (pos.x >= roomActiveArea.xMin && pos.x <= roomActiveArea.xMax)
        {
            if (pos.y <= roomActiveArea.yMin && pos.y >= roomActiveArea.yMax)
            {
                flag = true;
            }
        }
        return flag;
    }
}

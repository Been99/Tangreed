using UnityEngine;

// 모든 던전 방들의 가장 최상위 클래스
// 방의 모든 요소들(문, 함정, 몬스터, 스폰 포인트, 상자스폰 등)을 관리하고,
// 방이 시작되고 클리어되는 것에 맞춰 통제

public class BaseStage : MonoBehaviour
{
    // 던전에서 드랍되는 상자
    //public GameObject[] chestPrefabs;
    //public List<GameObject> chestList;

    // 몬스터 스폰 포인트
    //public MonsterSpawnPoint[] spawnPoint;

    public GameObject[] door;

    // 각 방들의 연결 정보
    public LinkedData stageLinkedData;

    public ROOMTYPE type;
    public ROOMCLASS sizeClass;
    public BoxCollider2D collider;

    // 방에대한 변수들
    public GameObject playerObj;
    public int nowFloor = 0;
    public int stageNum = -1;
    public bool roomLocked; // 몬스터를 다 잡으면 Lock이 풀린다.

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
        playerObj = GameObject.FindGameObjectWithTag("Player");
        collider = GetComponent<BoxCollider2D>();
        
        //spawnPoint = GetComponentsInChildren<MonsterSpawnPoint>();
        
        LoadDoors();
    }

    //private void Update()
    //{
    //    CheckRoomState();
    //     TODO :: 상자 나오는 로직 작성
    //}

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            NowPlayerEnter = true;
            CheckRoomState();
        }
    }

    public void LoadDoors()
    {
        // enum DOORTYPE 의 최대치로 배열 생성
        if (this.type == ROOMTYPE.Start) door = new GameObject[(int)DOORTYPE.Right];
        else door = new GameObject[(int)DOORTYPE.DoorMax];

        // 각 Door를 배열에 설정 후 비활성화
        for (int i = 0; i < door.Length; i++)
        {
            string doorName = $"Door{((DOORTYPE)i).ToString()}";
            GameObject doorObject = transform.Find(doorName)?.gameObject;
            if (doorObject != null)
            {
                door[i] = doorObject;
                door[i].SetActive(false);
            }

        }
    }

    // 자신의 아래에 있는 문들을 받아온다
    public void InitSetting()
    {
        //링크정보가 있고 해당 방향에 문도 존재하면 문을 만들어 준다.
        //문을 활성화 할때 맵정보에 문도 집어넣어 준다.
        if (stageLinkedData.rightMap != null)
        {
            if (door[(int)DOORTYPE.Right] != null)
            {
                door[(int)DOORTYPE.Right].gameObject.SetActive(true);
            }
        }
        if (stageLinkedData.leftMap != null)
        {
            if (door[(int)DOORTYPE.Left] != null)
            {
                door[(int)DOORTYPE.Left].gameObject.SetActive(true);
            }
        }
        if (stageLinkedData.upMap != null)
        {
            if (door[(int)DOORTYPE.Up] != null)
            {
                door[(int)DOORTYPE.Up].gameObject.SetActive(true);
            }
        }
        if (stageLinkedData.downMap != null)
        {
            if (door[(int)DOORTYPE.Down] != null)
            {
                door[(int)DOORTYPE.Down].gameObject.SetActive(true);
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
                Debug.Log("방에 들어옴");

                //아직 몬스터들이 스폰되지 않았거나 방이 클리어되지 않았을때 캐릭터의 입장을 검사한다.
                if (nowSpawned == false && roomIsClear == false)
                {
                    RoomStart();
                }

                //몹들이 스폰됐을때는 몬스터가 다 죽었는지를 검사한다.
                //if (nowSpawned == true)
                //{
                //    if (this.type != ROOMTYPE.Boss)
                //    {
                //        if (SpawnManager.Instance.NowSpawndList.Count <= 0)
                //        {
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

        //if (spawnPoint.Length != 0 && roomIsClear == false)로 교체
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
                        door[i].SetActive(true);
                    }
                }

            }
          
            //몬스터들을 스폰 시켜준다.
        }

        //스폰될 몬스터가 없는 방은 클리어 해준다.
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
                        door[i].SetActive(false);
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

    //}
}

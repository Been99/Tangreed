using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

// 던전을 생성한다.
// DFS탐색 알고리즘을 이용해서 맵이 만들어질 자리를 2차월 배열 공간에 미리 나열한 후
// 해당 방이 몇개의 방들과 어떻게 이어져 있는지에 따라서 실제 맵을 만든다.
// 연결되어있는 방의 개수에 따라 방의 크기가 랜덤으로 정해지고,
// 레스토랑, 보스방과 같은 특수방들도 조건에 따라 뽑아준다.

public class MapSpawner : Singleton<MapSpawner>
{
    // 문의 방향에 맞는 백터를 Dictionary값으로 정의
    public Dictionary<DOORDIRECTION, Vector2Int> dirIndex = new Dictionary<DOORDIRECTION, Vector2Int>()
    {
        { DOORDIRECTION.Up , new Vector2Int(0,1) },
        { DOORDIRECTION.Right, new Vector2Int(1,0) },
        { DOORDIRECTION.Down, new Vector2Int(0,-1) },
        { DOORDIRECTION.Left, new Vector2Int(-1,0) },
    };

    [Serializable]
    public class SpawnOption // 중첩 클래스, 스폰 옵션을 표시
    {
        [Tooltip("최대로 스폰될 방의 개수")]
        public int maxNum;

        [Tooltip("최소로 스폰될 방의 개수")]
        public int minNum;

        [Tooltip("하나의 층에 생성될 레스토랑의 개수")]
        public int maxRestaurant;

        [Range(0.00f, 1.00f), Tooltip("각 방의 브론즈박스 스폰 확률")]
        public float bronzeChestPercent;

        [Range(0.00f, 1.00f), Tooltip("각 방의 실버박스 스폰 확률")]
        public float silverChestPercent;

        [Range(0.00f, 1.00f), Tooltip("각 방의 골드박스 스폰 확률")]
        public float goldChestPercent;

        [Tooltip("정사각형 크기의 2차원 배열 크기")]
        public int maxListSize = 5;
    }

    [Serializable]
    public class CurrentValue // 중첩 클래스, 현재 값을 표시합니다.
    {
        public int NowCount;
        public int RestaurantCount;

        [SerializeField] public StageData[] mapList;
        public List<GameObject> currentRoomObjs;
        public GameObject[] mapObjList;
    }

    [SerializeField] public SpawnOption option = new SpawnOption();
    [SerializeField] public CurrentValue current = new CurrentValue();

    public MapManager mapManger;

    private void Awake()
    {
        InitSetting();

    }
    void Start()
    {
        SpawnStart(0);
    }

    // 초기 세팅
    public void InitSetting()
    {
        // StajeData와 Map에 배치될 Object의 정보를 초기화
        current.mapList = new StageData[option.maxListSize * option.maxListSize];
        current.mapObjList = new GameObject[option.maxListSize * option.maxListSize];
        for (int i = 0; i < option.maxListSize - 1; i++)
        {
            current.mapList[i] = null;
            current.mapObjList[i] = null;
        }
    }

    // 일반방들을 찾아서 그중 하나를 보스방으로 설정.
    public void SetBossRoom(STAGE stage)
    {
        int size = option.maxListSize;
        List<int> list = new List<int>();

        for (int i = 0; i < size * size; i++)
        {
            if (current.mapObjList[i] != null)
            {
                BaseStage baseStage = current.mapObjList[i].GetComponent<BaseStage>();
                if (baseStage.type == ROOMTYPE.Normal)
                {
                    list.Add(i);
                }
            }
        }

        int rnd = Random.Range(0, list.Count);
        int index = list[rnd];
        Destroy(current.mapObjList[index]);
        GameObject obj = MapManager.Instance.StageLoad(ROOMTYPE.Boss);
        current.mapObjList[index] = Instantiate(obj);
        current.mapObjList[index].GetComponent<BaseStage>().stageLinkedData = SetLinkingData(index);
    }

    //linkedData에서 어느방향이 연결되어 있는 지 확인하고
    //prefabs에서 해당방향에 방이 현재 만들어져 있는지 확인하고
    //서로 연결한다.
    public LinkedData SetLinkingData(int index)
    {
        LinkedData linkedData = new LinkedData();
        int yval = option.maxListSize;
        int x = index % yval;
        int y = index / yval;
        linkedData.num = current.mapList[index].Num;

        if (current.mapList[index].LeftMap != null)
        {
            if (current.mapObjList[(x - 1) + (y * yval)] != null)
            {
                linkedData.leftMap = current.mapObjList[(x - 1) + (y * yval)];
                if (current.mapObjList[(x - 1) + (y * yval)].GetComponent<BaseStage>().stageLinkedData != null)
                {
                    current.mapObjList[(x - 1) + (y * yval)].GetComponent<BaseStage>().stageLinkedData.rightMap = current.mapObjList[(x) + (y * yval)];
                }
            }
        }
        if (current.mapList[index].RightMap != null)
        {
            if (current.mapObjList[(x + 1) + (y * yval)] != null)
            {
                linkedData.rightMap = current.mapObjList[(x + 1) + (y * yval)];

                if (current.mapObjList[(x + 1) + (y * yval)].GetComponent<BaseStage>().stageLinkedData != null)
                {
                    current.mapObjList[(x + 1) + (y * yval)].GetComponent<BaseStage>().stageLinkedData.leftMap = current.mapObjList[(x) + (y * yval)];
                }
            }
        }
        if (current.mapList[index].UpMap != null)
        {
            if (current.mapObjList[x + ((y - 1) * yval)] != null)
            {
                linkedData.upMap = current.mapObjList[x + ((y - 1) * yval)];

                if (current.mapObjList[x + ((y - 1) * yval)].GetComponent<BaseStage>().stageLinkedData != null)
                {
                    current.mapObjList[x + ((y - 1) * yval)].GetComponent<BaseStage>().stageLinkedData.downMap = current.mapObjList[(x) + (y * yval)];
                }
            }
        }
        if (current.mapList[index].DownMap != null)
        {
            if (current.mapObjList[x + ((y + 1) * yval)] != null)
            {
                linkedData.downMap = current.mapObjList[x + ((y + 1) * yval)];

                if (current.mapObjList[x + ((y + 1) * yval)].GetComponent<BaseStage>().stageLinkedData != null)
                {
                    current.mapObjList[x + ((y + 1) * yval)].GetComponent<BaseStage>().stageLinkedData.upMap = current.mapObjList[(x) + (y * yval)];
                }
            }
        }

        return linkedData;
    }

    //시작위치, 레스토랑, 보스방 등의 방을 만들어 준다.
    public void StageSetting()
    {
        bool[] dir = new bool[(int)DOORTYPE.DoorMax];
        //direction.Initialize();
        bool flag = false;
        int size = option.maxListSize;
        for (int i = 0; i < size * size; i++)
        {
            int count = 0;
            flag = false;

            for (int b = 0; b < dir.Length; b++)
            {
                dir[b] = false;
            }

            if (current.mapList[i] != null)
            {
                //처음은 시작방
                if (current.mapList[i].Num == 0)
                {
                    GameObject obj = MapManager.Instance.StageLoad(ROOMTYPE.Start);
                    current.mapObjList[i] = Instantiate(obj);
                }
                else if (current.mapList[i].Num == current.NowCount - 1)//마지막 방은 끝방으로 한다.
                {
                    GameObject obj = MapManager.Instance.StageLoad(ROOMTYPE.End);
                    current.mapObjList[i] = Instantiate(obj);
                }
                else//0번방과 마지막 방을 빼고는 연결된 방의 개수(연결정보가 존재하는 문의 개수)에 따라 방의 클래스를 랜덤으로 정해준다.
                {
                    if (current.mapList[i].RightMap != null)
                    {
                        dir[(int)DOORTYPE.Right] = true;
                        count++;
                    }
                    if (current.mapList[i].LeftMap != null)
                    {
                        dir[(int)DOORTYPE.Left] = true;
                        count++;
                    }
                    if (current.mapList[i].UpMap != null)
                    {
                        dir[(int)DOORTYPE.Up] = true;
                        count++;
                    }
                    if (current.mapList[i].DownMap != null)
                    {
                        dir[(int)DOORTYPE.Down] = true;
                        count++;
                    }
                    //주변에 존재하는 연결된 방의 개수를 가지고 방의 크기를 결정한다.

                    //100% 큰방
                    if (count <= 2)//길이 하나 또는 두개인 방은 무조건 작은방 이면서 상점과, 음식점이 없으면 상점과 음식점이 된다.
                    {
                        int rnd = Random.Range(0, 100);
                        if (current.RestaurantCount < option.maxRestaurant)
                        {
                            GameObject obj = MapManager.Instance.StageLoad(ROOMTYPE.Restaurant);
                            current.mapObjList[i] = Instantiate(obj);

                            current.RestaurantCount++;
                            
                            //Debug.Log($"{current.Maplist[i].Num}번째방 레스토랑");
                        }
                        else
                        {
                            if (rnd < 70)
                            {
                                GameObject obj = MapManager.Instance.StageLoad(ROOMTYPE.Normal, ROOMCLASS.Small);
                                current.mapObjList[i] = Instantiate(obj);

                                //Debug.Log($"{current.Maplist[i].Num}번째방 작은방");
                            }
                            else
                            {
                                GameObject obj = MapManager.Instance.StageLoad(ROOMTYPE.Normal, ROOMCLASS.Medium);
                                current.mapObjList[i] = Instantiate(obj);
                                
                                //Debug.Log($"{current.Maplist[i].Num}번째방 중간방");
                            }

                        }
                    }
                    else if (count <= 3)//70%중간방,30%큰방
                    {
                        int rnd = Random.Range(0, 100);
                        if (rnd < 70)
                        {
                            GameObject obj = MapManager.Instance.StageLoad(ROOMTYPE.Normal, ROOMCLASS.Medium);
                            current.mapObjList[i] = Instantiate(obj);

                            //Debug.Log($"{current.Maplist[i].Num}번째방 중간방");
                        }
                        else
                        {
                            GameObject obj = MapManager.Instance.StageLoad(ROOMTYPE.Normal, ROOMCLASS.Large);
                            current.mapObjList[i] = Instantiate(obj);

                            //Debug.Log($"{current.Maplist[i].Num}번째방 큰방");
                        }
                    }
                    else if (count <= 4)//길이 두개있는방도 레스토랑과 상점방이 없으면 번호가 빠른 방이 레스토랑으로 뽑힌다.
                    {
                        GameObject obj = MapManager.Instance.StageLoad(ROOMTYPE.Normal, ROOMCLASS.Large);
                        current.mapObjList[i] = Instantiate(obj);

                        //Debug.Log($"{current.Maplist[i].Num}번째방 큰방");
                    }
                }
            }

            if (current.mapObjList[i] != null)
            {
                for (int a = 0; a < (int)DOORTYPE.DoorMax; a++)
                {
                    //문이 존재 해야 하는데 생성된 맵이 해당위치에 문이 없는 방이면 다시뽑게 한다.
                    if (dir[a])
                    {
                        if (current.mapObjList[i].GetComponent<BaseStage>().door[a] == null)
                        {
                            flag = true;
                            Destroy(current.mapObjList[i]);
                            break;
                        }
                    }
                }
            }
            if (flag)
            {
                i--;
                continue;
            }

            //프리팹 리스트에 값이 들어갔으면 주변에 있는 방들을 검사해서 링크데이터를 넣어준다.
            if (current.mapObjList[i] != null)
            {
                LinkedData data = SetLinkingData(i);
                current.mapObjList[i].GetComponent<BaseStage>().stageLinkedData = data;
               
                //Debug.Log($"{i}번방 링크세팅");
            }
        }
    }

    //스포너에서 맵을 생성을 해서 맵 배열을 만들어 주고 해당 배열을 맵 매니저에 넘겨준다.
    //public int MapSpawn(int x, int y, StageData Parent)
    public Vector2Int MapSpawn(int x, int y, StageData Parent, int depth)
    {
        //맵이 최대 개수까지 만들어 지면 종료한다.
        if (current.NowCount >= option.maxNum)
        {
            return new Vector2Int(x, y);
        }

        int RandNum;
        int yval = option.maxListSize;

        //들어오면 해당 위치에 방을 만들고 
        //음식점, 상점, 방 크기, 상자 스폰 등을 결정한다.
        //보스전이 있어야 하면 보스방도 스폰
        if (current.mapList[x + (y * yval)] == null)
        {
            current.mapList[x + (y * yval)] = new StageData();
            current.mapList[x + (y * yval)].InitSetting(current.NowCount, x, y);
            current.NowCount++;

            //각 방들은 서로 연결되어서 이동할 수 있어야 하지만 인접해 있다고 항상 연결되어 있어야 하는것은 아니다
            //따라서 탐색을 할때 자신이 현재 탐색한 위치에서 이전에 있었던 위치를 넘겨줌으로써
            //탐색을 수행한 경로가 방을 이어주는 통로가 될 수 있도록 해준다.
            if (Parent != null)
            {
                if (Parent.indexX == x)
                {
                    if (Parent.indexY > y) //아래쪽과 연결
                    {
                        current.mapList[x + (y * yval)].DownMap = Parent;
                        Parent.UpMap = current.mapList[x + (y * yval)];
                    }
                    else //위쪽과 연결
                    {
                        current.mapList[x + (y * yval)].UpMap = Parent;
                        Parent.DownMap = current.mapList[x + (y * yval)];


                    }
                }
                else if (Parent.indexY == y)
                {
                    if (Parent.indexX > x) //오른쪽과 연결
                    {
                        current.mapList[x + (y * yval)].RightMap = Parent;
                        Parent.LeftMap = current.mapList[x + (y * yval)];
                    }
                    else //왼쪽과 연결
                    {
                        current.mapList[x + (y * yval)].LeftMap = Parent;
                        Parent.RightMap = current.mapList[x + (y * yval)];
                    }
                }
            }

        }

        //천번째 방은 항상 오른쪽으로간다.
        if (current.NowCount == 1)
        {
            MapSpawn(x + 1, y, current.mapList[(x) + (y * yval)], depth + 1);
            //return option.MaxNum;
            return new Vector2Int(x + 1, y);
        }

        //왼쪽
        RandNum = Random.Range(0, 100);
        //Debug.Log($"랜덤{RandNum}");
        if (RandNum <= 70)
        {
            if (x - 1 >= 1 && current.mapList[(x - 1) + (y * yval)] == null)
            {
                MapSpawn(x - 1, y, current.mapList[(x) + (y * yval)], depth + 1);
            }
        }

        //오른쪽
        RandNum = Random.Range(0, 100);

        if (RandNum <= 70)
        {
            if (x + 1 <= option.maxListSize - 1 && current.mapList[(x + 1) + (y * yval)] == null)
            {
                MapSpawn(x + 1, y, current.mapList[(x) + (y * yval)], depth + 1);
            }
        }

        //위쪽
        RandNum = Random.Range(0, 100);
        if (RandNum <= 70)
        {
            if (y - 1 >= 0 && current.mapList[x + ((y - 1) * yval)] == null)
            {
                MapSpawn(x, y - 1, current.mapList[(x) + (y * yval)], depth + 1);
            }
        }

        //아래쪽
        RandNum = Random.Range(0, 100);

        if (RandNum <= 70)
        {
            if (y + 1 <= option.maxListSize - 1 && current.mapList[x + ((y + 1) * yval)] == null)
            {
                MapSpawn(x, y + 1, current.mapList[(x) + (y * yval)], depth + 1);
            }
        }

        // 확률로 움직이도록 하면 최소개수가 만들어 지지 않을수 있기 때문에 최소 개수가 채워지지 않으면 4 방향중 비어있는 곳을 찾아서 강제로 생성시켜 줍니다.
        if (current.NowCount < option.minNum)
        {
            for (int i = 0; i < 4; i++)
            {
                int tempx = x + dirIndex[(DOORDIRECTION)i].x;
                int tempy = y + dirIndex[(DOORDIRECTION)i].y;
                if (tempx >= 0 && tempx < option.maxListSize - 1 && tempy >= 0 && tempy < option.maxListSize - 1)
                {
                    if (current.mapList[tempx + ((tempy + 1) * yval)] == null)
                    {
                        MapSpawn(tempx, tempy, current.mapList[(tempx) + (tempy * yval)], depth + 1);
                    }
                }
            }
        }

        return new Vector2Int(x, y);
    }

    //맵스폰하고 스폰된 맵을 맵 매니저한테 넘겨준다.
    public void SpawnStart(int NowFloor)
    {
        current.NowCount = 0;
        //층수를 증가시키고맵을 만든자리를 DFS탐색 알고리즘을 통해서 뽑는다
        Vector2Int lastindex = MapSpawn(0, 2, null, 0);
        //맵을만들 자리 대로 맵을 실제로 생성해준다.
        StageSetting();

        //짝수번째 층에서는 보스방이 나타난다.
        if (MapManager.Instance.NowFloor % 2 == 0)
        {
            SetBossRoom(MapManager.Instance.NowStage);
        }

        //ShowMaps();

        MapManager.Instance.StageSetting(current.mapObjList, option.maxListSize);

        Debug.Log($"만들어진 방의 개수{current.NowCount}");
    }


    // TODO :: 미니맵 기능 구현
    //public void ShowMaps()
    //{
    //    int interval = 70; // 방 사이의 간격
    //    int yval = option.maxListSize; // 최대 리스트의 크기

    //    for (int y = 0; y < option.maxListSize; y++) // 2중 for문으로 각 맵의 위치를 순회하며 해당 위치에 맵 오브젝트가 있는지 확인하고 위치 설정
    //    {
    //        for (int x = 0; x < option.maxListSize; x++)
    //        {
    //            if (current.mapObjList[x + (y * yval)] != null)
    //            {
    //                GameObject obj = current.mapObjList[x + (y * yval)];
    //                obj.transform.position = new Vector3(transform.position.x + (x * interval), transform.position.y + ((y * interval) * -1));

    //                int num = obj.GetComponent<BaseStage>().stageLinkedData.num;
    //                obj.name = $"Room_{num}";

    //                obj.GetComponent<BaseStage>().stageNum = num;
    //            }
    //        }
    //    }
    //}
}

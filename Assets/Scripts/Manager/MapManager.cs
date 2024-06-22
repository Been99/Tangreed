using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 맵들을 관리하는 매니저 클래스
// 던전에 들어왔을 때 현재 스테이지에 따라 던전을 생성한다.
public class MapManager : Singleton<MapManager>
{
    public StageData[] linkedData = null;

    public GameObject[] stageArr = null;
    public int arrSize;

    [SerializeField] private STAGE nowStage = STAGE.Stage1;
    [SerializeField] private int nowFloor = 0;

    public GameObject[] specialRoom;
    public GameObject[] startRoom;
    public GameObject[] smallRoom;
    public GameObject[] mediumRoom;
    public GameObject[] largeRoom;
    public GameObject[] endRoom;

    // 2층단위로 보스가 나오므로, 1층을 제외, nowFloor가 홀수일 때 Stage를 증가시킨다
    public int NowFloor
    {
        get { return nowFloor; }
        set
        {
            nowFloor = value;
            if (nowFloor % 2 != 0 && nowFloor != 1)
            {
                nowStage = nowStage + 1;
            }
        }
    }

    public STAGE NowStage
    {
        get { return nowStage; }
        set
        {
            nowStage = value;
            // 스테이지 프리팹을 가져오는 로직
            // 현재 프로젝트에서는 (floor1 - Stage1)과 (floor2 - 첫번째 보스스테이지) 구현
            LoadStagePrefabs(value);
        }
    }

    private void Awake()
    {
        InitSetting();
        NowStage = STAGE.Stage1;
        NowFloor = 1;
    }

    // Resources의 Maps 폴더에서 가져온다.
    public void LoadStagePrefabs(STAGE stage)
    {
        largeRoom = Resources.LoadAll<GameObject>($"Maps/Large");
        mediumRoom = Resources.LoadAll<GameObject>($"Maps/Medium");
        smallRoom = Resources.LoadAll<GameObject>($"Maps/Small");

        specialRoom[(int)ROOMTYPE.Start] = Resources.Load<GameObject>($"Maps/Special/Map_Start");
        specialRoom[(int)ROOMTYPE.Restaurant] = Resources.Load<GameObject>($"Maps/Special/Map_Restaurant");
        specialRoom[(int)ROOMTYPE.End] = Resources.Load<GameObject>($"Maps/Special/Map_End");
        specialRoom[(int)ROOMTYPE.Boss] = Resources.Load<GameObject>($"Maps/Special/Map_Boss1");
    }

    public void StageSetting(GameObject[] rooms, int size)
    {
        stageArr = rooms;
        arrSize = size;

        for (int i = 0; i < size; i++)
        {
            for (int j = 0; j < size; j++)
            {
                if (rooms[i + (j * size)] != null)
                {
                    rooms[i + (j * size)]?.GetComponent<BaseStage>().InitSetting();
                }
            }
        }
    }

    public GameObject StageLoad(ROOMTYPE type)
    {
        return specialRoom[(int)type];
    }

    public GameObject StageLoad(ROOMTYPE type, ROOMCLASS roomclass)
    {
        //랜덤으로 뽑아서 하나를 넘겨준다.
        int count = 0;
        if (roomclass == ROOMCLASS.Small) count = smallRoom.Length;
        else if (roomclass == ROOMCLASS.Medium) count = mediumRoom.Length;
        else if (roomclass == ROOMCLASS.Large) count = largeRoom.Length;

        int rnd = Random.Range(0, count);

        if (roomclass == ROOMCLASS.Small) return smallRoom[rnd];
        else if (roomclass == ROOMCLASS.Medium) return mediumRoom[rnd];
        else return largeRoom[rnd];

    }

    public void InitSetting()
    {
        specialRoom = new GameObject[(int)ROOMTYPE.Max];
    }

    //현재 만들어져 있는  스테이지를 모두 없애주고 새로운 방들을 만들어 준다.
    public void NextStage()
    {
        DestroyRooms(stageArr);
        NowFloor++;
        MapSpawner.Instance.SpawnStart(NowFloor);
    }

    public void DestroyRooms(GameObject[] stage)
    {
        for (int i = 0; i < stage.Length; i++)
        {
            Destroy(stage[i]);
        }
    }
}
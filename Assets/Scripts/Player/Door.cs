using System;
using UnityEngine;
using UnityEngine.Tilemaps;


// 던전의 문 객체
// 이어져있는 다른 던전방의 정보를 가지고 있음
// 문이 활성화 되어있을 때 캐릭터가 문의 트리거에 접촉하면 해당 방의 이어져 있는 문으로 위치가 이동
public class Door : MonoBehaviour
{
    public Tilemap wallTilemap;
    public Tilemap backTilemap;
    public Tilemap movableTilemap;

    public Tilemap mainWallTilemap;
    public Tilemap mainBackTilemap;
    public Tilemap mainMovableTilemap;

    public Tile tile;

    public Transform startPos;
    public Transform endPos;

    public Vector3Int doorStartIndex;
    public Vector3Int doorEndIndex;

    public Vector3Int mainStartIndex;
    public Vector3Int mainEndIndex;

    public Vector2Int size;

    public GameObject nextRoom;

    public LayerMask playerMask;
    public DOORTYPE type;

    public GameObject parentStage;

    public bool nowDoorLocked;

    public ParticleSystem doorParticle;

    [Header("==================Test==================")]
    public Transform mapInfoPos;
    public Vector3Int startIndex;
    public Vector3Int parentStartIndex;


    public bool NowDoorLocked
    {
        get { return nowDoorLocked; }
        set
        {
            nowDoorLocked = value;
            if (doorParticle != null && nowDoorLocked) doorParticle.gameObject.SetActive(false);
        }
    }

    void Start()
    {
        wallTilemap = null;
        backTilemap = null;
        movableTilemap = null;


        mainWallTilemap = null;
        mainBackTilemap = null;
        mainMovableTilemap = null;

        doorParticle = GetComponentInChildren<ParticleSystem>();

        NowDoorLocked = false;
    }

    // 문이 생성될 타일맵의 인덱스번호를 넘겨주면 해당 인덱스의 위치에서 문을 생성한다.
    public void CreateDoor(GameObject tilemaps)
    {
        parentStage = tilemaps;

        Tilemap[] temp = tilemaps.GetComponentsInChildren<Tilemap>();
        foreach (var i in temp)
        {
            if (i.gameObject.layer == LayerMask.NameToLayer("Wall"))
            {
                mainWallTilemap = i;
            }
            else if (i.gameObject.layer == LayerMask.NameToLayer("BackGround"))
            {
                mainBackTilemap = i; // BackGround 타일은 아직 미구현
            }
            else if (i.gameObject.layer == LayerMask.NameToLayer("Movable"))
            {
                mainMovableTilemap = i; // Movable 아직 미구현
            }
        }

        if (this.type == DOORTYPE.Up)
        {
            nextRoom = tilemaps.GetComponent<BaseStage>().stageLinkedData.upMap;
        }
        else if (this.type == DOORTYPE.Down)
        {
            nextRoom = tilemaps.GetComponent<BaseStage>().stageLinkedData.downMap;
        }
        else if (this.type == DOORTYPE.Right)
        {
            nextRoom = tilemaps.GetComponent<BaseStage>().stageLinkedData.rightMap;
        }
        else
        {
            nextRoom = tilemaps.GetComponent<BaseStage>().stageLinkedData.leftMap;
        }

        GetTileInfo(type);

        // 문이 생성될 위치에 TileMap을 비워준다.
        for (int i = 0; i < size.y; i++)
        {
            for (int j = 0; j < size.x; j++)
            {
                Vector3Int index = new Vector3Int(j, i * -1, 0);
                index = mainStartIndex + index;

                mainWallTilemap.SetTile(index, null);

                mainBackTilemap.SetTile(index, null);

                if (movableTilemap != null)
                {
                    mainMovableTilemap.SetTile(index, null);
                }
            }
        }
        
        BaseStage stageData = tilemaps.GetComponent<BaseStage>();
        SetStageDoorInfo();
    }

    // 문이 만들어지면 해당 문이 위치하는 맵의 정보에 문에대한 값을 넣어준다.
    private void SetStageDoorInfo()
    {
        int x = 0;
        int y = 0;
        RaycastHit2D[] hit;
        int size = parentStage.GetComponent<BaseStage>().maxX;
        // WorldToCell : Tilemap 에서 제공하는 메서드로 월드좌표의 타일맵을 셀 좌표로 변환
        startIndex = mainWallTilemap.WorldToCell(mapInfoPos.position);
        // GetCellCenterWorld : Tilemap 에서 제공하는 메서드로 타일맵 내 특정 셀의 중심 위치를 월드 좌표로 변환
        // 이를 이용하여 문을 배치
        Vector3 temp = mainWallTilemap.GetCellCenterWorld(startIndex);

    }

    // 문 타입에 따른 정보를 받아서 전달
    // 가로세로 크기, 정보가 있는 셀의 시작 인덱스번호 등
    private void GetTileInfo(DOORTYPE type)
    {
        Tilemap[] temp = GetComponentsInChildren<Tilemap>(); // 자기 자신의 타일맵 정보를 받아온다.

        foreach (var i in temp)
        {
            if (i.gameObject.layer == LayerMask.NameToLayer("Wall"))
            {
                wallTilemap = i;
            }
            else if (i.gameObject.layer == LayerMask.NameToLayer("BackGround"))
            {
                backTilemap = i;
            }
            else if (i.gameObject.layer == LayerMask.NameToLayer("Moveable"))
            {
                movableTilemap = i;
            }
        }

        startPos = transform.Find("srartPos");
        endPos = transform.Find("endPos");

        doorStartIndex = wallTilemap.WorldToCell(startPos.position);
        doorEndIndex = wallTilemap.WorldToCell(endPos.position);

        mainStartIndex = mainWallTilemap.WorldToCell(startPos.position);
        mainEndIndex = mainWallTilemap.WorldToCell(endPos.position);

        // door의 size에 맞춰 재수정
        size.x = doorEndIndex.x - doorStartIndex.x + 1;
        size.y = doorStartIndex.y - doorEndIndex.y + 1;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!nowDoorLocked)
        {
            if (collision.gameObject.layer == LayerMask.NameToLayer("Player"))
            {
                GoNextMap(collision.gameObject);
            }
        }
    }

    // 현재 DOORTYPE에 따른 현재맵에서 다음맵으로 이동
    private void GoNextMap(GameObject Palyer)
    {
        Door pos = null;
        GameObject temp = null;
        if (this.type == DOORTYPE.Up)
        {
            temp = parentStage.GetComponent<BaseStage>().stageLinkedData.upMap;
            pos = nextRoom.GetComponent<BaseStage>().door[(int)DOORTYPE.Down];
        }
        else if (this.type == DOORTYPE.Down)
        {
            temp = parentStage.GetComponent<BaseStage>().stageLinkedData.downMap;
            pos = nextRoom.GetComponent<BaseStage>().door[(int)DOORTYPE.Up];
        }
        else if (this.type == DOORTYPE.Right)
        {
            temp = parentStage.GetComponent<BaseStage>().stageLinkedData.rightMap;
            pos = nextRoom.GetComponent<BaseStage>().door[(int)DOORTYPE.Left];
        }
        else
        {
            temp = parentStage.GetComponent<BaseStage>().stageLinkedData.leftMap;
            pos = nextRoom.GetComponent<BaseStage>().door[(int)DOORTYPE.Right];
        }
        string name = temp.gameObject.name;

        nextRoom = GameObject.Find(name);

        parentStage.GetComponent<BaseStage>().NowPlayerEnter = false;

        nextRoom.GetComponent<BaseStage>().NowPlayerEnter = true;
    }
}

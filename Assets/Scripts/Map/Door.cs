using System;
using UnityEngine;
using UnityEngine.Tilemaps;


// 던전의 문 객체
// 이어져있는 다른 던전방의 정보를 가지고 있음
// 문이 활성화 되어있을 때 캐릭터가 문의 트리거에 접촉하면 해당 방의 이어져 있는 문으로 위치가 이동
public class Door : MonoBehaviour
{
    public GameObject nextRoom;

    public DOORTYPE type;

    private GameObject currentStage;

    public bool nowDoorLocked;

    public LayerMask playerLayer;

    public ParticleSystem doorParticle;

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
        doorParticle = GetComponentInChildren<ParticleSystem>();

        NowDoorLocked = false;
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
        GameObject pos = null;
        GameObject temp = null;
        if (this.type == DOORTYPE.Up)
        {
            temp = currentStage.GetComponent<BaseStage>().stageLinkedData.upMap;
            pos = nextRoom.GetComponent<BaseStage>().door[(int)DOORTYPE.Down];
        }
        else if (this.type == DOORTYPE.Down)
        {
            temp = currentStage.GetComponent<BaseStage>().stageLinkedData.downMap;
            pos = nextRoom.GetComponent<BaseStage>().door[(int)DOORTYPE.Up];
        }
        else if (this.type == DOORTYPE.Right)
        {
            temp = currentStage.GetComponent<BaseStage>().stageLinkedData.rightMap;
            pos = nextRoom.GetComponent<BaseStage>().door[(int)DOORTYPE.Left];
        }
        else
        {
            temp = currentStage.GetComponent<BaseStage>().stageLinkedData.leftMap;
            pos = nextRoom.GetComponent<BaseStage>().door[(int)DOORTYPE.Right];
        }
        string name = temp.gameObject.name;

        nextRoom = GameObject.Find(name);

        currentStage.GetComponent<BaseStage>().NowPlayerEnter = false;

        nextRoom.GetComponent<BaseStage>().NowPlayerEnter = true;
    }
}
